using UnityEngine;

/// <summary>
/// Điều khiển nòng pháo (turret) xoay theo vị trí chuột
/// Gắn script này vào GameObject con chứa sprite nòng pháo (Gun/Turret)
/// GameObject nòng pháo phải là CON của thân xe (Tank Hull)
/// </summary>
public class TankTurret : MonoBehaviour
{
    [Header("Bắn đạn (tuỳ chọn)")]
    [Tooltip("Prefab đạn cần bắn")]
    public GameObject bulletPrefab;

    [Tooltip("Vị trí xuất hiện đạn (đầu nòng)")]
    public Transform firePoint;

    [Tooltip("Phím bắn")]
    public KeyCode fireKey = KeyCode.Mouse0;

    [Tooltip("Tốc độ bắn (viên/giây)")]
    public float fireRate = 1f;

    [Header("Hiệu ứng bắn (tuỳ chọn)")]
    public ParticleSystem muzzleFlash;
    public AudioClip fireSound;

    // Private
    private float nextFireTime = 0f;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && fireSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Bắn đạn khi nhấn phím (nếu có cấu hình bullet)
        if (bulletPrefab != null && Input.GetKey(fireKey) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    /// <summary>
    /// Bắn đạn
    /// </summary>
    void Fire()
    {
        if (firePoint == null)
        {
            Debug.LogWarning("TankTurret: Chưa gán FirePoint!");
            return;
        }

        // Tạo đạn tại đầu nòng
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Hiệu ứng lửa
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Âm thanh
        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
    }

    // Vẽ đường ngắm trong Scene view
    void OnDrawGizmos()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firePoint.position, transform.up * 2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(firePoint.position, 0.1f);
    }
}