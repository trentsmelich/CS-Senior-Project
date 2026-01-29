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
        int fenceLayer = LayerMask.GetMask("Default");
    
        // Check each direction - store the collider result to avoid multiple calls
        Collider2D upCollider = Physics2D.OverlapCircle(transform.position + Vector3.up * gridSize, checkRadius, fenceLayer);
        up = upCollider != null && upCollider.GetComponent<Fence>() != null;
        Debug.Log($"Up: {up} (collider: {upCollider != null}, fence: {(upCollider != null ? (upCollider.GetComponent<Fence>() != null).ToString() : "N/A")})");
    
        Collider2D downCollider = Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius, fenceLayer);
        down = downCollider != null && downCollider.GetComponent<Fence>() != null;
        Debug.Log($"Down: {down} (collider: {downCollider != null}, fence: {(downCollider != null ? (downCollider.GetComponent<Fence>() != null).ToString() : "N/A")})");
    
        Collider2D leftCollider = Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius, fenceLayer);
        left = leftCollider != null && leftCollider.GetComponent<Fence>() != null;
        Debug.Log($"Left: {left} (collider: {leftCollider != null}, fence: {(leftCollider != null ? (leftCollider.GetComponent<Fence>() != null).ToString() : "N/A")})");
    
        Collider2D rightCollider = Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius, fenceLayer);
        right = rightCollider != null && rightCollider.GetComponent<Fence>() != null;
        Debug.Log($"Right: {right} (collider: {rightCollider != null}, fence: {(rightCollider != null ? (rightCollider.GetComponent<Fence>() != null).ToString() : "N/A")})");
        
        Debug.Log($"Checking positions from {transform.position}: Up={transform.position + Vector3.up * gridSize}, Down={transform.position + Vector3.down * gridSize}, Left={transform.position + Vector3.left * gridSize}, Right={transform.position + Vector3.right * gridSize}");
        //Debug.Log($"Collider found: {upCollider.gameObject.name}");
    }
    private void AdjustFenceSprite()
    {
        checkAroundFence();
        if (up)
        {
            //get child in fence and activate game object FenceUp
            transform.Find("FenceUp").gameObject.SetActive(true);
            Debug.Log("Activated FenceUp");

        }
        if (down)
        {
            //get child in fence and activate game object FenceDown
            transform.Find("FenceDown").gameObject.SetActive(true);
            Debug.Log("Activated FenceDown");
        }
        if (left)
        {
            //get child in fence and activate game object FenceLeft
            transform.Find("FenceLeft").gameObject.SetActive(true);
            Debug.Log("Activated FenceLeft");
        }
        if (right)
        {
            //get child in fence and activate game object FenceRight
            transform.Find("FenceRight").gameObject.SetActive(true);
            Debug.Log("Activated FenceRight");
        }
    }
    
}
