using UnityEngine;
using System.Collections;

public class IceSpike : MonoBehaviour
{
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float activationDelay = 0.1f;
    [SerializeField] private AnimationClip destroyAfterClip;

    private void Start()
    {
        if (hitCollider != null)
        {
            hitCollider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay());
        }

        if (destroyAfterClip != null)
        {
            StartCoroutine(DestroyAfterClip());
        }
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        if (activationDelay > 0f)
        {
            yield return new WaitForSeconds(activationDelay);
        }

        if (hitCollider != null)
        {
            hitCollider.enabled = true;
        }
    }

    private IEnumerator DestroyAfterClip()
    {
        yield return new WaitForSeconds(destroyAfterClip.length);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            var playerHealth = collider.GetComponent<PlayerStats>();
            if (playerHealth != null) playerHealth.TakeDamage(damage);
        }
    }
}
