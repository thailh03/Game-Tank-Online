using UnityEngine;

/// <summary>
/// Hiển thị số damage bay lên khi trúng đạn
/// Gắn script này vào Prefab DamagePopup
/// Prefab cần: SpriteRenderer + script này
/// </summary>
public class DamagePopup : MonoBehaviour
{
    [Header("Animation")]
    [Tooltip("6 frame sprite popup (nhỏ -> to -> bay lên -> mờ)")]
    public Sprite[] popupFrames;

    [Tooltip("Thời gian mỗi frame (giây)")]
    public float frameDuration = 0.08f;

    [Tooltip("Bay lên bao nhiêu px (world unit)")]
    public float floatDistance = 1.2f;

    [Tooltip("Sprite normal damage")]
    public Sprite[] normalFrames;

    [Tooltip("Sprite critical damage")]
    public Sprite[] critFrames;

    private SpriteRenderer sr;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isCrit = false;
    private Vector3 startPos;

    // Static factory method — tạo popup từ bất kỳ script nào
    public static void Create(GameObject prefab, Vector3 worldPos, int damage, bool isCrit = false)
    {
        if (prefab == null) return;
        var go = Instantiate(prefab, worldPos + Vector3.up * 0.3f, Quaternion.identity);
        var popup = go.GetComponent<DamagePopup>();
        if (popup != null)
            popup.Setup(isCrit);
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    public void Setup(bool crit)
    {
        isCrit = crit;
        currentFrame = 0;
        timer = 0f;

        Sprite[] frames = crit ? critFrames : normalFrames;
        if (frames != null && frames.Length > 0)
            sr.sprite = frames[0];
    }

    void Update()
    {
        Sprite[] frames = isCrit ? critFrames : normalFrames;
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                Destroy(gameObject);
                return;
            }

            sr.sprite = frames[currentFrame];
        }

        // Bay lên theo thời gian
        float progress = (float)currentFrame / frames.Length;
        transform.position = startPos + Vector3.up * (floatDistance * progress);

        // Fade alpha ở nửa sau
        if (progress > 0.5f)
        {
            float alpha = 1f - ((progress - 0.5f) / 0.5f);
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
