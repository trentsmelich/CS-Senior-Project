using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;
//Author:Trent and Jia
//Description: This script manages the building state of the game, including tower placement and validation.
public class BuildingState : GameState
{
    private GameObject towerToPlace;
    private GameObject previewTower;
    private Grid grid;

    private Tilemap grassTilemap;
    private Tilemap grassTilemap2;
    private Tilemap dirtTilemap;
    private Grid grid2;
    private Grid grid3;

    private LayerMask buildingLayer = LayerMask.GetMask("Default");

    public override void EnterState(GameStateController Game)
    {
        Game.GetBuildingScreen().SetActive(true);
        towerToPlace = Game.GetPlaceTower();

        previewTower = GameObject.Instantiate(towerToPlace);
        previewTower.GetComponent<Collider2D>().enabled = false;
        previewTower.GetComponent<TowerAI>().enabled = false;

        // Make preview semi transparent by getting all sprite rendereres in the gameobject and setting a to 0.5f
        SpriteRenderer[] renderers = previewTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            Color c = renderer.color;
            c.a = 0.5f;
            renderer.color = c;
        }

        grid = Game.GetGrid();
        grassTilemap = Game.GetGrassTilemap();
        grassTilemap2 = Game.GetGrassTilemap2();
        dirtTilemap = Game.GetDirtTilemap();
        grid2 = Game.GetGrid2();
        grid3 = Game.GetGrid3();
    }

    public override void UpdateState(GameStateController Game)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = grid.WorldToCell(mousePos);
        Vector3 cellCenter = grid.GetCellCenterWorld(cell);
        
        //check for corners of the tower to see if they are grass
        Vector3Int cellTopRight = new Vector3Int(cell.x + 1, cell.y + 1, cell.z);
        Vector3Int cellTopLeft = new Vector3Int(cell.x - 1, cell.y + 1, cell.z);
        Vector3Int cellBottomRight = new Vector3Int(cell.x + 1, cell.y - 1, cell.z);
        Vector3Int cellBottomLeft = new Vector3Int(cell.x - 1, cell.y - 1, cell.z);

        previewTower.transform.position = cellCenter;

        // Check if the cell is grass and not dirt
        bool validTile = grassTilemap.HasTile(cellTopRight) && grassTilemap.HasTile(cellTopLeft) && grassTilemap.HasTile(cellBottomRight) && grassTilemap.HasTile(cellBottomLeft) && grassTilemap.HasTile(cell) && !dirtTilemap.HasTile(cell)||
                         grassTilemap2.HasTile(cellTopRight) && grassTilemap2.HasTile(cellTopLeft) && grassTilemap2.HasTile(cellBottomRight) && grassTilemap2.HasTile(cellBottomLeft) && grassTilemap2.HasTile(cell) && !dirtTilemap.HasTile(cell);
                         

        // Check if the cell is overllapping with other buildings
        bool blocked = IsBlocked(cellCenter);


        if (towerToPlace.CompareTag("Fence"))
        {
            validTile = true;
        }
        bool canBuild = validTile && !blocked;

        // Change color based on if the cell is valid or not (green = valid, red = not valid)
        SpriteRenderer[] renderers = previewTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            Color c = renderer.color;
            c.a = 0.5f;

            if (canBuild)
            {
                c.r = 0f; c.g = 1f; c.b = 0f; // green
            }
            else
            {
                c.r = 1f; c.g = 0f; c.b = 0f; // red
            }

            renderer.color = c;
        }

        // PLACE TOWER IF U CAN BUILD IT THEN CREATE THE OBJECT AND SET THE GAMESTATE BACK TO IDLE
        if (Input.GetMouseButtonDown(0) && canBuild)
        {
            GameObject placedTower = GameObject.Instantiate(towerToPlace, cellCenter, Quaternion.identity);

            // placed building must be in Buildings layer
            placedTower.layer = 0;

            placedTower.GetComponent<TowerParent>().increaseCount();

            SpriteRenderer[] towerRenderers = placedTower.GetComponentsInChildren<SpriteRenderer>();
            int num = 0;
            // Set sorting order based on the y position of the cell so that buildings closer to the bottom of the screen are 
            // rendered in front of buildings closer to the top of the screen
            foreach (SpriteRenderer renderer in towerRenderers)
            {
                renderer.sortingOrder = Mathf.RoundToInt(-cellCenter.y) + num;
                num++;
            }
            //if placed tower is a fence allow player to immediately place another tower without having to go back to shop state
            if (placedTower.CompareTag("Fence"))
            {
                PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
                if(Game.GetCurrentBuildingCost() <= playerStats.coins)
                {
                    Game.PlayPlaceBuildingSFX();
                    playerStats.coins -= Game.GetCurrentBuildingCost();
                    GameObject.Destroy(previewTower);
                    EnterState(Game);

                }
                else
                {
                    Game.SetPlaceTower(null);
                    Game.SetState(new gameIdleState());
                    
                }
                

            }
            else
            {
                // Set the game state back to idle
                Game.PlayPlaceBuildingSFX();
                CheckForSuperBuilding(placedTower);
                Game.SetPlaceTower(null);
                Game.SetState(new gameIdleState());
            }
            
        }

        // Cancel building state if right mouse button is pressed and set the game state back to idle
        if (Input.GetMouseButtonDown(1))
        {
            Game.PlayButtonClickSound();
            Game.AddBackBuildingCoins(Game.GetCurrentBuildingCost());
            Game.SetPlaceTower(null);
            Game.SetState(new gameIdleState());
        }
    }

    public override void ExitState(GameStateController Game)
    {
        Game.GetBuildingScreen().SetActive(false);
        GameObject.Destroy(previewTower);
    }

    //Check if the cell is blocked by other buildings
    private bool IsBlocked(Vector3 center)
    {
        BoxCollider2D box = towerToPlace.GetComponent<BoxCollider2D>();

        if (box == null)
        {
            return false;
        }

        // Calculate box size & offset in world space
        Vector2 size = box.size;
        Vector2 offset = box.offset;

        Vector2 worldCenter = (Vector2)center + offset;
        //Check if the collider of the placing tower overlaps with other colliders
        Collider2D hit = Physics2D.OverlapBox(
            worldCenter,
            size,
            0f,
            buildingLayer
        );
        //return true if hit found a collider, false if it did not
        return hit != null;
    }

    //When a building is placed check to see if surrounding buildings are in an orientation to create SUPER buildings.
    //If they are then find building to be powered up and make it powerful idk wtf to do here yet. 
    private void CheckForSuperBuilding(GameObject buildingPlaced)
    {
        if(buildingPlaced.GetComponent<TowerParent>().TowerName == "SlingShot" && buildingPlaced.GetComponent<TowerParent>().Level == 3)
        {
            if(checkSlingShotPowerUp(buildingPlaced))
            {
                
                //find the slingshot tower and set it to super mode
                buildingPlaced.GetComponent<SlingShotTower>().SetSuperMode();
            }
        }
        if(buildingPlaced.GetComponent<TowerParent>().TowerName == "Catapult" && buildingPlaced.GetComponent<TowerParent>().Level == 3)
        {
            Collider2D hit = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(-2, 0, 0), 0.1f, buildingLayer);
            if(hit != null)
            {
                if(checkSlingShotPowerUp(hit.gameObject))
                {
                    //find the slingshot tower and set it to super mode
                    hit.gameObject.GetComponent<SlingShotTower>().SetSuperMode();
                }
            

            }

            if(checkCatapultPowerUp(buildingPlaced))
            {
                //find the catapult tower and set it to super mode
                buildingPlaced.GetComponent<Catapult>().SetSuperMode();
            }
        }
        if(buildingPlaced.GetComponent<StatModifier>() != null)
        {
            if(buildingPlaced.GetComponent<StatModifier>().getStatModifier() == "Damage")
            {

                Collider2D hit = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(0, 2, 0), 0.1f, buildingLayer);
                if(hit != null)
                {
                    if(checkSlingShotPowerUp(hit.gameObject))
                    {
                        //find the slingshot tower and set it to super mode
                        hit.gameObject.GetComponent<SlingShotTower>().SetSuperMode();
                    }
                    if(checkCatapultPowerUp(hit.gameObject))
                    {
                    //find the slingshot tower and set it to super mode
                        hit.gameObject.GetComponent<Catapult>().SetSuperMode();
                    }

                }
            }
            if(buildingPlaced.GetComponent<StatModifier>().getStatModifier() == "Speed")
            {

                Collider2D hit = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(-2, 2, 0), 0.1f, buildingLayer);
                if(hit != null)
                {
                    if(checkSlingShotPowerUp(hit.gameObject))
                    {
                        //find the slingshot tower and set it to super mode
                        hit.gameObject.GetComponent<SlingShotTower>().SetSuperMode();
                    }
                    if(checkCatapultPowerUp(hit.gameObject))
                    {
                        //find the slingshot tower and set it to super mode
                        hit.gameObject.GetComponent<Catapult>().SetSuperMode();
                    }
                

                }
            }
            
        }
        if(buildingPlaced.GetComponent<TowerParent>().TowerName == "Freezer" && buildingPlaced.GetComponent<TowerParent>().Level == 3)
        {
            Collider2D hit = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(-2, 0, 0), 0.1f, buildingLayer);
            if(hit != null)
            {
                if(checkCatapultPowerUp(hit.gameObject))
                {
                    //find the slingshot tower and set it to super mode
                    hit.gameObject.GetComponent<Catapult>().SetSuperMode();
                }
            

            }
        }
    }
    private bool checkSlingShotPowerUp(GameObject buildingPlaced)
    {
        bool one = false;
        bool two = false;
        bool three = false;

        if(buildingPlaced.GetComponent<TowerParent>().TowerName  == "SlingShot" && buildingPlaced.GetComponent<TowerParent>().Level == 3)
        {

            //check down, down right, and right for slingshot super building
            Collider2D hit1 = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(0, -2, 0), 0.1f, buildingLayer);
            Collider2D hit2 = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(2, -2, 0), 0.1f, buildingLayer);
            Collider2D hit3 = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(2, 0, 0), 0.1f, buildingLayer);
            

            if(hit1 != null && hit2 != null && hit3 != null)
            {
                //check if hit1 has component of statmodifier return bool
                if(hit1.GetComponent<StatModifier>() != null)
                {
                    if(hit1.GetComponent<StatModifier>().getStatModifier() == "Damage")
                    {
                        one = true; 
                    }
                }
                if(hit2.GetComponent<StatModifier>() != null)
                {
                    if(hit2.GetComponent<StatModifier>().getStatModifier() == "Speed")
                    {
                        two = true; 
                    }
                }
                if(hit3.GetComponent<Catapult>() != null)
                {
                    if(hit3.GetComponent<Catapult>().Level == 3)
                    {
                        three = true;
                    }
                }
                

                
            }
            
        }
        return (one && two && three);
    }
    private bool checkCatapultPowerUp(GameObject buildingPlaced)
    {
        bool one = false;
        bool two = false;
        bool three = false;

        if(buildingPlaced.GetComponent<TowerParent>().TowerName  == "Catapult" && buildingPlaced.GetComponent<TowerParent>().Level == 3)
        {

            //check up, up right, and right for catapult super building
            Collider2D hit1 = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(0, -2, 0), 0.1f, buildingLayer);
            Collider2D hit2 = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(2, -2, 0), 0.1f, buildingLayer);
            Collider2D hit3 = Physics2D.OverlapCircle(buildingPlaced.transform.position + new Vector3(2, 0, 0), 0.1f, buildingLayer);
            

            if(hit1 != null && hit2 != null && hit3 != null)
            {
                if(hit1.GetComponent<StatModifier>() != null)
                {
                    if(hit1.GetComponent<StatModifier>().getStatModifier() == "Damage")
                    {
                        one = true; 
                    }
                }
                if(hit2.GetComponent<StatModifier>() != null)
                {
                    if(hit2.GetComponent<StatModifier>().getStatModifier() == "Speed")
                    {
                        two = true; 
                    }
                }
                if(hit3.GetComponent<Freezer>() != null)
                {
                    if(hit3.GetComponent<Freezer>().Level == 3)
                    {
                        three = true;
                    }
                }
                

                
            }
            
        }
        return (one && two && three);

    

    }
}
