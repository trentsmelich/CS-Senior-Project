using UnityEngine;
using UnityEngine.Tilemaps;


public class BuildingState : GameState
{
    private GameObject towerToPlace;
    private Tilemap tilemap;
    private GameObject previewTower;
    private Grid grid;
    private Grid grid2;
    private Tilemap grassTilemap;
    private Tilemap grassTilemap2;
    public override void EnterState(GameStateController Game)
    {
        towerToPlace = Game.GetPlaceTower();
        previewTower = GameObject.Instantiate(towerToPlace);
        previewTower.GetComponent<Collider2D>().enabled = false;
        previewTower.GetComponent<TowerAI>().enabled = false;
        SpriteRenderer[] renderers = previewTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = 0.5f; // Set alpha to 50%
            renderer.color = color;
        }
        grid = Game.GetGrid();
        grid2 = Game.GetGrid();
        grassTilemap = Game.GetGrassTilemap();
        grassTilemap2 = Game.GetGrassTilemap2();

    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the building state
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = grid.WorldToCell(mousePosition);
        Vector3 cellCenterPos = grid.GetCellCenterWorld(cell);
        previewTower.transform.position = cellCenterPos;


        bool canBuild = grassTilemap.HasTile(cell) || grassTilemap2.HasTile(cell);
        SpriteRenderer[] renderers = previewTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = 0.5f; // Set alpha to 50%
            if (canBuild)
            {
                color.g = 1f;
                color.r = 0f;
            }
            else
            {
                color.g = 0f;
                color.r = 1f;
            }
            renderer.color = color;
        }

        if (Input.GetMouseButtonDown(0) && canBuild)
        {
            // Place tower
            GameObject placedTower = GameObject.Instantiate(towerToPlace, cellCenterPos, Quaternion.identity);
            
            placedTower.GetComponent<TowerParent>().increaseCount();
            if (Game.GetUnlockController() == null)
            {
                Debug.Log("Youre stupid");
            }
            Game.GetUnlockController().CheckUnlocks();

            // Debug unlock status

            foreach (UnlockParent unlock in Game.GetUnlockController().GetUnlocks())
            {
                if (unlock is SlingshotUnlock sling)
                {
                    Debug.Log($"Slingshot Unlocks: L1={sling.lvl1Unlocked}, L2={sling.lvl2Unlocked}, L3={sling.lvl3Unlocked}");
                }
                else if (unlock is CatapultUnlock catapult)
                {
                    Debug.Log($"Catapult Unlocks: L1={catapult.lvl1Unlocked}, L2={catapult.lvl2Unlocked}, L3={catapult.lvl3Unlocked}");
                }
            }

            // end of debug

            Debug.Log("Placed Tower: " +  Game.GetUnlockController().GetNumTowers("SlingShot", 1));

            Game.SetPlaceTower(null);
            Game.SetState(new gameIdleState());
        }
        else if (Input.GetMouseButtonDown(1) )
        {
            // Cancel placement
            Game.SetPlaceTower(null);
            Game.SetState(new gameIdleState());
        }
    }

    public override void ExitState(GameStateController Game)
    {
        // Implementation for exiting the building state
        GameObject.Destroy(previewTower);
        
    }
}