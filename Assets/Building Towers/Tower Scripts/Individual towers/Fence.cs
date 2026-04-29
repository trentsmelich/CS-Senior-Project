using UnityEngine;

public class Fence : TowerParent
{
    
    // Start is called before the first frame update
    private bool up, down, left, right;
    
    

    void Start()
    {
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
        return  "Attributes\n" +
                "Level:"+ "<pos=125>" + level.ToString() + "</pos>\n" + "\n" +
                "Hit Points:" + "<pos=125>" + health.ToString() + "</pos>\n" + "\n" +
                "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
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
    
        Collider2D downCollider = Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius, fenceLayer);
        down = downCollider != null && downCollider.GetComponent<Fence>() != null;
    
    
        Collider2D leftCollider = Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius, fenceLayer);
        left = leftCollider != null && leftCollider.GetComponent<Fence>() != null;
        
    
        Collider2D rightCollider = Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius, fenceLayer);
        right = rightCollider != null && rightCollider.GetComponent<Fence>() != null;
        
    }
    private void AdjustFenceSprite()
    {
        checkAroundFence();
        if (up)
        {
            //get child in fence and activate game object FenceUp
            transform.Find("FenceUp").gameObject.SetActive(true);
        }
        else
        {
            //get child in fence and deactivate game object FenceUp
            transform.Find("FenceUp").gameObject.SetActive(false);
        }
        if (down)
        {
            //get child in fence and activate game object FenceDown
            transform.Find("FenceDown").gameObject.SetActive(true);
        }
        else
        {
            //get child in fence and deactivate game object FenceDown
            transform.Find("FenceDown").gameObject.SetActive(false);
        }
        if (left)
        {
            //get child in fence and activate game object FenceLeft
            transform.Find("FenceLeft").gameObject.SetActive(true);
        }
        else
        {
            //get child in fence and deactivate game object FenceLeft
            transform.Find("FenceLeft").gameObject.SetActive(false);
        }
        if (right)
        {
            //get child in fence and activate game object FenceRight
            transform.Find("FenceRight").gameObject.SetActive(true);
        }
        else
        {
            //get child in fence and deactivate game object FenceRight
            transform.Find("FenceRight").gameObject.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        // update fences around this fence to adjust their sprites when this fence is destroyed
        checkAroundFence();
        float gridSize = 1f; // Distance between fence positions
        float checkRadius = 0.5f; // Small radius to check
        int fenceLayer = LayerMask.GetMask("Default");
    
        // Check each direction - store the collider result to avoid multiple calls
        
        if(up)
        {
            Collider2D upCollider = Physics2D.OverlapCircle(transform.position + Vector3.up * gridSize, checkRadius, fenceLayer);
            upCollider.GetComponent<Fence>().AdjustFenceSprite();
        }
        if(down)
        {
            Collider2D downCollider = Physics2D.OverlapCircle(transform.position + Vector3.down * gridSize, checkRadius, fenceLayer);
            downCollider.GetComponent<Fence>().AdjustFenceSprite();
        }
        if(left)
        {
            Collider2D leftCollider = Physics2D.OverlapCircle(transform.position + Vector3.left * gridSize, checkRadius, fenceLayer);
            leftCollider.GetComponent<Fence>().AdjustFenceSprite();
        }
        if(right)
        {
            Collider2D rightCollider = Physics2D.OverlapCircle(transform.position + Vector3.right * gridSize, checkRadius, fenceLayer);
            rightCollider.GetComponent<Fence>().AdjustFenceSprite();
        }
        
    }

}
