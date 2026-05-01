// [Vị trí: Assets/Scripts/Thai/TankHealth.cs]
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [Header("--- Chỉ số Sinh Tồn ---")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("--- Giao Diện (UI) ---")]
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Cập nhật tụt vạch máu
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " đã nổ tung!");
        Destroy(gameObject);
    }
}