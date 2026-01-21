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
            // Set the game state back to idle
            //Play Building SFX???
            Game.SetPlaceTower(null);
            Game.SetState(new gameIdleState());
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
        GameObject.Destroy(previewTower);
    }

    //Check if the cell is blocked by other buildings
    private bool IsBlocked(Vector3 center)
    {
        BoxCollider2D box = towerToPlace.GetComponent<BoxCollider2D>();

        if (box == null)
        {
            Debug.LogWarning("Tower prefab missing BoxCollider2D.");
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
}
