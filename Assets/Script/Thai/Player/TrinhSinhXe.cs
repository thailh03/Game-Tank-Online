// [Vị trí: Assets/Scripts/Thai/Managers/TrinhSinhXe.cs]
using UnityEngine;

public class TrinhSinhXe : MonoBehaviour
{
    [Header("--- KHO DỮ LIỆU THẬT ---")]
    [Tooltip("Kéo các Prefab xe tank xanh dương vào đây (Thứ tự phải giống hệt Menu)")]
    public GameObject[] danhSachPrefabXe;

    [Tooltip("Kéo các Prefab đạn thật vào đây (Thứ tự phải giống hệt Menu)")]
    public GameObject[] danhSachPrefabDan;

    [Header("--- ĐIỂM XUẤT HIỆN ---")]
    [Tooltip("Kéo một Object trống trên bản đồ làm điểm rơi của xe")]
    public Transform diemSinhXe;

    void Start()
    {
        // 1. Đọc số liệu lưu trong sổ xé nháp (CauHinhGame)
        int indexXe = CauHinhGame.ChiSoXeChon;
        int indexDan = CauHinhGame.ChiSoDanChon;
        string tenNguoiChoi = CauHinhGame.TenNguoiChoi;

        // 2. Kiểm tra an toàn xem có Prefab nào bị thiếu không
        if (danhSachPrefabXe.Length > indexXe && danhSachPrefabDan.Length > indexDan)
        {
            // 3. Đẻ ra chiếc xe tank tại vị trí xuất phát
            GameObject xeCuaThai = Instantiate(danhSachPrefabXe[indexXe], diemSinhXe.position, diemSinhXe.rotation);

            // Đổi tên xe thành "Lê Hoàng Thái" (hoặc tên bạn gõ ngoài Menu)
            xeCuaThai.name = tenNguoiChoi;

            // 4. QUAN TRỌNG NHẤT: NẠP ĐẠN CHO XE
            TankController scriptXe = xeCuaThai.GetComponent<TankController>();

            if (scriptXe != null)
            {
                // Nhét thẳng viên đạn từ kho vào nòng súng của chiếc xe vừa đẻ ra
                scriptXe.bulletPrefab = danhSachPrefabDan[indexDan];
            }
            else
            {
                Debug.LogError("Cảnh báo: Chiếc xe này không có gắn script TankController!");
            }
        }
    }
}