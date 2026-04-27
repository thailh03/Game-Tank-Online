using UnityEngine;

/// <summary>
/// Điều khiển di chuyển thân xe tank trên MapTileA
/// Gắn script này vào GameObject chứa sprite thân xe (Hull)
/// </summary>
public class TankMovement : MonoBehaviour
{
    [Header("Di chuyển")]
    [Tooltip("Tốc độ di chuyển của xe tank")]
    public float moveSpeed = 5f;

    [Tooltip("Tốc độ xoay thân xe")]
    public float rotateSpeed = 180f;

    [Header("Giới hạn Map (MapTileA)")]
    [Tooltip("Bật giới hạn biên map")]
    public bool useBoundary = true;

    [Tooltip("Góc trên bên trái của map")]
    public Vector2 mapMin = new Vector2(-10f, -10f);

    [Tooltip("Góc dưới bên phải của map")]
    public Vector2 mapMax = new Vector2(10f, 10f);

    [Header("Hiệu ứng")]
    [Tooltip("Hiệu ứng bụi khi di chuyển (tuỳ chọn)")]
    public ParticleSystem dustEffect;

    // Components
    private Rigidbody2D rb;
    private float moveInput;
    private float rotateInput;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Cấu hình Rigidbody2D cho tank
        rb.gravityScale = 0f;
        rb.angularDamping = 10f;
        rb.linearDamping = 5f;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    void Update()
    {
        // Nhận input từ bàn phím
        // W/S hoặc Up/Down: tiến/lùi
        moveInput = Input.GetAxis("Vertical");
        // A/D hoặc Left/Right: xoay thân xe
        rotateInput = Input.GetAxis("Horizontal");

        isMoving = Mathf.Abs(moveInput) > 0.01f;

        // Hiệu ứng bụi
        HandleDustEffect();
    }

    void FixedUpdate()
    {
        // Xoay thân xe (A/D)
        float rotation = -rotateInput * rotateSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotation);

        // Di chuyển tiến/lùi theo hướng xe đang quay
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            Vector2 direction = transform.up * moveInput;
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

            // Giới hạn trong phạm vi map
            if (useBoundary)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, mapMin.x, mapMax.x);
                newPosition.y = Mathf.Clamp(newPosition.y, mapMin.y, mapMax.y);
            }

            rb.MovePosition(newPosition);
        }
    }

    void HandleDustEffect()
    {
        if (dustEffect == null) return;

        if (isMoving && !dustEffect.isPlaying)
            dustEffect.Play();
        else if (!isMoving && dustEffect.isPlaying)
            dustEffect.Stop();
    }

    // Vẽ giới hạn map trong Scene view để dễ chỉnh
    void OnDrawGizmosSelected()
    {
        if (!useBoundary) return;
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((mapMin.x + mapMax.x) / 2f, (mapMin.y + mapMax.y) / 2f, 0f);
        Vector3 size = new Vector3(mapMax.x - mapMin.x, mapMax.y - mapMin.y, 0f);
        Gizmos.DrawWireCube(center, size);
    }
}