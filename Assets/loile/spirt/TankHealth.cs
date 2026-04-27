using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Quản lý máu xe tank + cập nhật HP Bar sprite
/// Gắn vào GameObject Tank (Hull)
/// </summary>
public class TankHealth : MonoBehaviour
{
    [Header("Máu")]
    public int maxHP = 100;
    public int currentHP;

    [Header("HP Bar Sprite (6 trạng thái)")]
    [Tooltip("Gán SpriteRenderer của thanh máu")]
    public SpriteRenderer hpBarRenderer;

    [Tooltip("6 sprite theo thứ tự: 100% / 80% / 60% / 40% / 20% / 0%")]
    public Sprite[] hpSprites = new Sprite[6];  // HP_100pct -> HP_0pct

    [Header("Hiệu ứng chết")]
    public GameObject explosionPrefab;
    public AudioClip hitSound;
    public AudioClip destroySound;

    private AudioSource audioSource;
    private bool isDead = false;

    void Awake()
    {
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        UpdateHPBar();
    }

    /// <summary>
    /// Gọi hàm này khi xe bị trúng đạn
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP = Mathf.Max(0, currentHP - damage);
        UpdateHPBar();

        // Âm thanh trúng đạn
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentHP <= 0)
            Die();
    }

    /// <summary>
    /// Cập nhật sprite HP Bar theo % máu hiện tại
    /// </summary>
    void UpdateHPBar()
    {
        if (hpBarRenderer == null || hpSprites == null || hpSprites.Length < 6) return;

        float ratio = (float)currentHP / maxHP;

        // Map ratio -> index sprite
        // 100%=0, 80%=1, 60%=2, 40%=3, 20%=4, 0%=5
        int index;
        if      (ratio > 0.9f) index = 0;
        else if (ratio > 0.7f) index = 1;
        else if (ratio > 0.5f) index = 2;
        else if (ratio > 0.3f) index = 3;
        else if (ratio > 0f)   index = 4;
        else                   index = 5;

        hpBarRenderer.sprite = hpSprites[index];
    }

    void Die()
    {
        isDead = true;

        if (audioSource != null && destroySound != null)
            audioSource.PlayOneShot(destroySound);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Tắt xe (hoặc Destroy sau delay)
        Invoke(nameof(DestroyTank), 0.5f);
    }

    void DestroyTank()
    {
        Destroy(gameObject);
    }

    // Lấy % máu (0.0 -> 1.0)
    public float GetHPRatio() => (float)currentHP / maxHP;
    public bool IsDead() => isDead;
}
