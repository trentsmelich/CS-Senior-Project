using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingState : GameState
{
    private GameObject towerToPlace;
    private GameObject previewTower;
    private Grid grid;

    private Tilemap grassTilemap;
    private Tilemap grassTilemap2;

    // Layer for placed buildings (YOU MUST SET THIS IN UNITY)
    private LayerMask buildingLayer = LayerMask.GetMask("Default");

    public override void EnterState(GameStateController Game)
    {
        towerToPlace = Game.GetPlaceTower();

        previewTower = GameObject.Instantiate(towerToPlace);
        previewTower.GetComponent<Collider2D>().enabled = false;
        previewTower.GetComponent<TowerAI>().enabled = false;

        // Make preview semi-transparent
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

        // --------- CHECK VALID TILE ---------
        bool validTile = grassTilemap.HasTile(cellTopRight) && grassTilemap.HasTile(cellTopLeft) && grassTilemap.HasTile(cellBottomRight) && grassTilemap.HasTile(cellBottomLeft) ||
                         grassTilemap2.HasTile(cellTopRight) && grassTilemap2.HasTile(cellTopLeft) && grassTilemap2.HasTile(cellBottomRight) && grassTilemap2.HasTile(cellBottomLeft);

        // --------- CHECK OVERLAP WITH OTHER BUILDINGS ---------
        bool blocked = IsBlocked(cellCenter);

        bool canBuild = validTile && !blocked;

        // ---- Change color based on validity ----
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

        // --------- PLACE BUILDING ---------
        if (Input.GetMouseButtonDown(0) && canBuild)
        {
            GameObject placedTower = GameObject.Instantiate(towerToPlace, cellCenter, Quaternion.identity);

            // The placed building must be in "Buildings" layer
            placedTower.layer = 0;

            placedTower.GetComponent<TowerParent>().increaseCount();

            SpriteRenderer[] towerRenderers = placedTower.GetComponentsInChildren<SpriteRenderer>();
            int num = 0;
            foreach (SpriteRenderer renderer in towerRenderers)
            {
                renderer.sortingOrder = Mathf.RoundToInt(-cellCenter.y) + num;
                num++;
            }

            Game.SetPlaceTower(null);
            Game.SetState(new gameIdleState());
        }

        // --------- CANCEL ---------
        if (Input.GetMouseButtonDown(1))
        {
            Game.SetPlaceTower(null);
            Game.SetState(new gameIdleState());
        }
    }

    public override void ExitState(GameStateController Game)
    {
        GameObject.Destroy(previewTower);
    }

    // ===========================================================
    //                  BLOCK CHECK (important)
    // ===========================================================

    private bool IsBlocked(Vector3 center)
    {
        // I assume your building prefabs use a BoxCollider2D
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

        Collider2D hit = Physics2D.OverlapBox(
            worldCenter,
            size,
            0f,
            buildingLayer
        );

        return hit != null;
    }
}
