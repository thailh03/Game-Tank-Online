// [Vị trí: Assets/Scripts/Thai/Bullet.cs]
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("--- Chỉ số Đạn ---")]
    public int damage = 20; // Sức công phá của viên đạn này

    private float speed;
    private GameObject shooter; // Lưu lại người bắn để đạn không tự nổ vào mặt mình
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Hàm này được TankController gọi ngay lúc viên đạn đẻ ra
    public void Setup(GameObject owner, float bulletSpeed)
    {
        shooter = owner;
        speed = bulletSpeed;

        // Tạo lực đẩy viên đạn bay thẳng theo hướng nòng súng
        rb.linearVelocity = transform.up * speed;

        // Dọn rác: Tự hủy sau 3 giây nếu đạn bay ra ngoài map không trúng gì cả
        Destroy(gameObject, 3f);
    }

    // Xử lý khi viên đạn đập vào một vật thể khác (yêu cầu Collider để Is Trigger)
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Nếu đạn chạm ngay vào người vừa bắn ra nó -> Bỏ qua
        if (hitInfo.gameObject == shooter) return;

        // 2. Kiểm tra xem nạn nhân có gắn script TankHealth không?
        TankHealth tankHealth = hitInfo.GetComponent<TankHealth>();

        if (tankHealth != null)
        {
            // Trừ máu nạn nhân
            tankHealth.TakeDamage(damage);
        }

        // 3. Đạn nổ: Sau khi đập trúng xe hoặc trúng tường thì đạn tự hủy
        Destroy(gameObject);
    }
}