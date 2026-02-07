using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float bulletSpeed = 50f;
    public float bulletLife = 5f;
    private float damage = 25f;
    private Vector3 direction = Vector3.forward;
    private Rigidbody rb;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        Destroy(gameObject, bulletLife);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && player.IsAlive())
            {
                player.TakeDamage(damage);
                CreateHitEffect(collision.transform.position);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            // Enemy hit logic (implement later)
            CreateHitEffect(collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("Ground"))
        {
            CreateHitEffect(collision.transform.position);
            Destroy(gameObject);
        }
    }

    void CreateHitEffect(Vector3 position)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, position, Quaternion.identity);
        }

        if (hitSound != null && AudioListener.volume > 0)
        {
            AudioSource.PlayClipAtPoint(hitSound, position);
        }
    }

    public float GetDamage()
    {
        return damage;
    }
}