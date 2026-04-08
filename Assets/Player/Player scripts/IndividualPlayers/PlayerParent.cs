using UnityEngine;

public abstract class PlayerParent : MonoBehaviour
{
    [SerializeField] protected float projDelay = 0.3f;
    public float ProjDelay => projDelay;
    public abstract void Attack(PlayerStateController player);
}
