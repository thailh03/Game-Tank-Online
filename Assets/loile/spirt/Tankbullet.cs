using UnityEngine;

/// <summary>
/// Script đạn pháo - gắn vào Prefab đạn
/// </summary>
public class TankBullet : MonoBehaviour
{
    [Header("Đạn")]
    public float speed = 15f;
    public float lifetime = 3f;
    public int damage = 10;

    [Header("Critical Hit")]
    [Range(0f, 1f)]
    public float critChance = 0.2f;    // 20% xác suất crit
    public float critMultiplier = 2f;   // crit = damage x2

    [Header("Hiệu ứng")]
    public GameObject explosionPrefab;
    public GameObject damagePopupPrefab;  // Prefab có DamagePopup.cs

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
    }

    void Start()
    {
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Bullet")) return;

        // Kiểm tra có TankHealth không
        TankHealth health = other.GetComponent<TankHealth>();
        if (health != null)
        {
            // Tính damage (crit?)
            bool isCrit = Random.value < critChance;
            int finalDamage = isCrit ? Mathf.RoundToInt(damage * critMultiplier) : damage;

            // Gây damage
            health.TakeDamage(finalDamage);

            // Hiển thị số damage bay lên
            if (damagePopupPrefab != null)
                DamagePopup.Create(damagePopupPrefab, transform.position, finalDamage, isCrit);
        }

        // Hiệu ứng nổ
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
