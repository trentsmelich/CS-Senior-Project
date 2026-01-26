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
    private void AdjustFenceSprite()
    {
        
    }
}
