using UnityEngine;

public class RootPrj : MonoBehaviour
{
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 5f;

    private bool didHitThisOut = false;

    private void Start()
    {
        if (hitCollider != null)
            hitCollider.enabled = false;

        Destroy(gameObject, lifetime);
    }

    public void EnableDamage()
    {
        didHitThisOut = false;
        if (hitCollider != null)
            hitCollider.enabled = true;
    }

    public void DisableDamage()
    {
        if (hitCollider != null)
            hitCollider.enabled = false;
    }

    public void DestroyRoot()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitCollider == null || !hitCollider.enabled) return;
        if (didHitThisOut) return;

        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                didHitThisOut = true;
            }
        }
    }
}