using UnityEngine;

public class Fence : TowerParent
{
    
    // Start is called before the first frame update
    private bool up, down, left, right;
    
    

    void Start()
    {
        Debug.Log("Fence Tower Created");
        //Once fence is placed check up, down, left, right for adjacent fences
        //If adjacent fence found, adjust this fence's sprite to connect with adjacent fence
        AdjustFenceSprite();

    }

    // UpdateTower is called every frame to update tower behavior
    public override void UpdateTower(Transform enemy)
    {
        // Fences do not attack
    }

    public override string GetName()
    {
        return towerName.ToString();
    }
    public override string GetDescription()
    {
        return "Basic fence that blocks enemy movement.";
    }
    public override string GetAttributes()
    {
        return "none lmao rn";
    }
    // AdjustFenceSprite checks for adjacent fences and updates the sprite accordingly
    private void checkAroundFence()
    {
        float gridSize = 1f; // Distance between fence positions
        float checkRadius = 0.5f; // Small radius to check
    
        // Check each direction
        up = Physics2D.OverlapCircle(transform.position + Vector3.up * gridSize, checkRadius) != null 
            && Physics2D.OverlapCircle(transform.position + Vector3.up * gridSize, checkRadius).GetComponent<Fence>() != null;
            //debug log both check bool checks
            Debug.Log($"Up Check 1: {Physics2D.OverlapCircle(transform.position + Vector3.up * gridSize, checkRadius) != null}");
            Debug.Log($"Up Check 2: {Physics2D.OverlapCircle(transform.position + Vector3.up * gridSize, checkRadius)?.GetComponent<Fence>() != null}");

    
        down = Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius) != null
            && Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius).GetComponent<Fence>() != null;
            Debug.Log($"Down Check 1: {Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius) != null}");
            Debug.Log($"Down Check 2: {Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius)?.GetComponent<Fence>() != null}");
    
        left = Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius) != null
            && Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius).GetComponent<Fence>() != null;
            Debug.Log($"Left Check 1: {Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius) != null}");
            Debug.Log($"Left Check 2: {Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius)?.GetComponent<Fence>() != null}");
    
        right = Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius) != null
            && Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius).GetComponent<Fence>() != null;
            Debug.Log($"Right Check 1: {Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius) != null}");
            Debug.Log($"Right Check 2: {Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius)?.GetComponent<Fence>() != null}");
        Debug.Log($"Fence connections - Up: {up}, , Down: {down}, Left: {left}, Right: {right}");
        //check position where checking
        Debug.Log($"Checking Up Position: {transform.position + Vector3.up * gridSize}");
        Debug.Log($"Checking Down Position: {transform.position + Vector3.down * gridSize}");
        Debug.Log($"Checking Left Position: {transform.position + Vector3.left * gridSize}");
        Debug.Log($"Checking Right Position: {transform.position + Vector3.right * gridSize}");
    }
    private void AdjustFenceSprite()
    {
        checkAroundFence();
        if (up)
        {
            //get child in fence and activate game object FenceUp
            transform.Find("FenceUp").gameObject.SetActive(true);

        }
        if (down)
        {
            //get child in fence and activate game object FenceDown
            transform.Find("FenceDown").gameObject.SetActive(true);
        }
        if (left)
        {
            //get child in fence and activate game object FenceLeft
            transform.Find("FenceLeft").gameObject.SetActive(true);
        }
        if (right)
        {
            //get child in fence and activate game object FenceRight
            transform.Find("FenceRight").gameObject.SetActive(true);
        }
    }
    
}
