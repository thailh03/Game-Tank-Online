// [Vị trí: Assets/Scripts/Player/TrinhSinhXe.cs]
using UnityEngine;

public class TrinhSinhXe : MonoBehaviour
{
    public GameObject[] danhSachXePrefab;
    public Transform diemSinhXe;

    void Start()
    {
        int index = CauHinhGame.ChiSoXeChon;
        GameObject xeDuocChon = Instantiate(danhSachXePrefab[index], diemSinhXe.position, diemSinhXe.rotation);

        // Gán tên đã đặt cho xe
        xeDuocChon.name = CauHinhGame.TenNguoiChoi;

        Debug.Log("Đã tạo xe: " + xeDuocChon.name + " với loại đạn số: " + CauHinhGame.ChiSoDanChon);
    }
}