using UnityEngine;
using System.Collections;

public class IceSpike : MonoBehaviour
{
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private float damage = 10f;
    [SerializeField] private AnimationClip destroyAfterClip;

    private IEnumerator Start()
    {
        if (destroyAfterClip != null)
        {
            yield return new WaitForSeconds(destroyAfterClip.length);
            Destroy(gameObject);
        }
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
