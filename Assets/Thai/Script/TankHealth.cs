using UnityEngine;

public class TankHealth : MonoBehaviour, IDamageable
{
    [Header("Chỉ số máu")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Hiệu ứng")]
    public GameObject explosionPrefab; // Kéo hiệu ứng nổ vào đây

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " còn lại: " + currentHealth + " máu.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Thay vì xóa ngay, có thể làm xác xe hoặc ẩn đi để tránh lỗi Null
        Destroy(gameObject);
    }
}
