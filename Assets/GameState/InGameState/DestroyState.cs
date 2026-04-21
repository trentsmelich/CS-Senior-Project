using UnityEngine;

public class DestroyState : GameState
{
    private const float clickRadius = 0.2f;
    private Color previousBackgroundColor;

    public override void EnterState(GameStateController Game)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }

        Game.GetDestroyScreen().SetActive(true);

        previousBackgroundColor = cam.backgroundColor;
        cam.backgroundColor = Color.red;
    }

    public override void UpdateState(GameStateController Game)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                return;
            }

            Vector2 clickWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapCircleAll(clickWorldPos, clickRadius);
            if (hits.Length == 0)
            {
                return;
            }

            PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStats>();

            foreach (Collider2D hit in hits)
            {
                if (hit == null)
                {
                    continue;
                }

                TowerParent towerParent = hit.GetComponentInParent<TowerParent>();
                if (towerParent == null)
                {
                    continue;
                }

                GameObject target = towerParent.gameObject;
                if (!target.CompareTag("Tower") && !target.CompareTag("Fence"))
                {
                    continue;
                }

                if (playerStats != null)
                {
                    int refund = Mathf.RoundToInt(towerParent.TowerCost * 0.8f);
                    playerStats.coins += refund;
                }

                GameObject.Destroy(target);
                break;
            }
        }
    }

    public override void ExitState(GameStateController Game)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }

        cam.backgroundColor = previousBackgroundColor;
        Game.GetDestroyScreen().SetActive(false);
    }
}
