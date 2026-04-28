// [Vị trí: Assets/Scripts/Thai/TankController.cs]
using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("--- Di Chuyển ---")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;

    [Header("--- Bộ Phận & Vũ Khí ---")]
    public Rigidbody2D rb;
    [Tooltip("Kéo tất cả các điểm nòng súng vào đây. Xe 2 nòng thì kéo 2 điểm.")]
    public Transform[] firePoints;

    [HideInInspector] // Ẩn khỏi Inspector vì TrinhSinhXe sẽ tự động nạp đạn vào đây!
    public GameObject bulletPrefab;

    public float bulletSpeed = 15f;
    public float fireRate = 0.5f;

    private Vector2 moveInput;
    private float nextFireTime = 0f;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Đảm bảo xe không bị rớt do trọng lực
        rb.gravityScale = 0;
        // Đảm bảo xe không tự xoay khi va chạm
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 1. Lấy đầu vào di chuyển
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. Bắn đạn (Chuột trái)
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Di chuyển tịnh tiến
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

        // Xoay toàn bộ thân xe và nòng theo hướng di chuyển
        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void Shoot()
    {
        if (firePoints == null || firePoints.Length == 0) return;

        // Lặp qua tất cả các nòng súng để bắn
        foreach (Transform fp in firePoints)
        {
            // Kiểm tra xem hệ thống đã nạp đạn vào cho xe chưa
            if (fp != null && bulletPrefab != null)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, fp.position, fp.rotation);
                Bullet bulletScript = bulletObj.GetComponent<Bullet>();

                if (bulletScript != null)
                {
                    // Truyền 'this.gameObject' để viên đạn biết ai là chủ (Online-ready)
                    bulletScript.Setup(this.gameObject, bulletSpeed);
                }
            }
        }
    }
}