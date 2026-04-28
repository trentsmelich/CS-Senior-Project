using UnityEngine;
using System.Collections;

public abstract class PowerUpParent : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveHeight = 0.2f;
    [SerializeField] protected float moveSpeed = 2f;

    [Header("Lifetime")]
    [SerializeField] protected float lifetime = 15f;
    [SerializeField] protected float blinkStartTime = 5f;
    [SerializeField] protected float blinkInterval = 0.2f;

    protected Vector3 startPos;
    protected SpriteRenderer spriteRenderer;
    protected bool collected = false;

    protected PlayerStats player;
    protected AudioSource powerUpSFX;

  
    void Start()
    {
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        powerUpSFX = GameObject.Find("SFX/PowerUp_SFX").GetComponent<AudioSource>();
        StartCoroutine(LifetimeRoutine());
    }

    
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerStats>();
            if (player == null) return;

            ApplyEffect(player);
            collected = true;
            Destroy(gameObject);
        }
    }

    protected abstract void ApplyEffect(PlayerStats player);

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime - blinkStartTime);

        float elapsed = 0f;
        while (elapsed < blinkStartTime)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        Destroy(gameObject);
    }
}
