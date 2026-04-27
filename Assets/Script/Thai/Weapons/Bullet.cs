using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f;
    private GameObject owner;
    private bool isReadyToHitOwner = false;

    public void Setup(GameObject shooter, float speed)
    {
        owner = shooter;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Rigidbody2D shooterRb = shooter.GetComponent<Rigidbody2D>();

        // Cộng vận tốc xe vào đạn để đạn luôn nhanh hơn xe
        Vector2 extraVelocity = (shooterRb != null) ? shooterRb.linearVelocity : Vector2.zero;
        rb.linearVelocity = (Vector2)transform.up * speed + extraVelocity;

        Invoke("EnableSelfDamage", 0.1f); // 0.1s bảo hiểm
        Destroy(gameObject, 3f);
    }

    void EnableSelfDamage() => isReadyToHitOwner = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner && !isReadyToHitOwner) return;

        IDamageable hitObj = other.GetComponent<IDamageable>();
        if (hitObj != null)
        {
            hitObj.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}