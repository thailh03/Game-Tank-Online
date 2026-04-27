using UnityEngine;

/// <summary>
/// Điều khiển nòng pháo (turret) 2 nòng - bắn 2 viên cùng lúc
/// Gắn script này vào GameObject con chứa sprite nòng pháo (Gun/Turret)
/// GameObject nòng pháo phải là CON của thân xe (Tank Hull)
/// </summary>
public class TankTurret2 : MonoBehaviour
{
    [Header("Bắn đạn")]
    [Tooltip("Prefab đạn cần bắn")]
    public GameObject bulletPrefab;

    [Tooltip("FirePoint nòng TRÁI (đầu nòng bên trái)")]
    public Transform firePointL;

    [Tooltip("FirePoint nòng PHẢI (đầu nòng bên phải)")]
    public Transform firePointR;

    [Tooltip("Phím bắn")]
    public KeyCode fireKey = KeyCode.Mouse0;

    [Tooltip("Tốc độ bắn (lần/giây)")]
    public float fireRate = 1f;

    [Header("Hiệu ứng bắn (tuỳ chọn)")]
    [Tooltip("Muzzle flash nòng trái")]
    public ParticleSystem muzzleFlashL;

    [Tooltip("Muzzle flash nòng phải")]
    public ParticleSystem muzzleFlashR;

    public AudioClip fireSound;

    // Private
    private float nextFireTime = 0f;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && fireSound != null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (bulletPrefab != null && Input.GetKey(fireKey) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    /// <summary>
    /// Bắn 2 viên đạn cùng lúc từ 2 nòng
    /// </summary>
    void Fire()
    {
        bool fired = false;

        if (firePointL != null)
        {
            Instantiate(bulletPrefab, firePointL.position, firePointL.rotation);
            if (muzzleFlashL != null) muzzleFlashL.Play();
            fired = true;
        }
        else
        {
            Debug.LogWarning("TankTurret: Chưa gán FirePointL!");
        }

        if (firePointR != null)
        {
            Instantiate(bulletPrefab, firePointR.position, firePointR.rotation);
            if (muzzleFlashR != null) muzzleFlashR.Play();
            fired = true;
        }
        else
        {
            Debug.LogWarning("TankTurret: Chưa gán FirePointR!");
        }

        // Âm thanh (1 lần dù bắn 2 viên)
        if (fired && audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
    }

    // Vẽ đường ngắm cả 2 nòng trong Scene view
    void OnDrawGizmos()
    {
        if (firePointL != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(firePointL.position, firePointL.up * 2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(firePointL.position, 0.1f);
        }

        if (firePointR != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(firePointR.position, firePointR.up * 2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(firePointR.position, 0.1f);
        }
    }
}