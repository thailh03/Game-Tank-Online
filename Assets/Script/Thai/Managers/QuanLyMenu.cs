// [Vị trí thư mục: Assets/Scripts/Thai/Managers/QuanLyMenu.cs]
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuanLyMenu : MonoBehaviour
{
    [Header("--- GIAO DIỆN NHẬP LIỆU ---")]
    public TMP_InputField oNhapTen;
    public Button nutVaoGame;

    [Header("--- KỸ THUẬT RENDER TEXTURE (CHỤP PREFAB XE) ---")]
    [Tooltip("Kéo danh sách PREFAB xe vào đây")]
    public GameObject[] danhSachPrefabXe;

    [Tooltip("Kéo Object Diem_DatXe vào đây")]
    public Transform diemDatXe;

    // Biến để nhớ xe nào đang đứng trong phòng chụp để lát xóa đi
    private GameObject xeDangDungTrongPhongChup;

    [Header("--- KHO HÌNH ẢNH (CHỌN ĐẠN) ---")]
    [Tooltip("Kéo Img_Dan trên Canvas vào đây")]
    public Image imgHienThiDan;

    [Tooltip("Kéo các tấm ảnh (Sprite) của viên đạn vào đây")]
    public Sprite[] mangHinhDan;

    void Start()
    {
        // Mặc định chọn xe 0 và đạn 0
        CauHinhGame.ChiSoXeChon = 0;
        CauHinhGame.ChiSoDanChon = 0;

        CapNhatGiaoDienHinhAnh();
        KiemTraTenHopLe();
    }

    // ==========================================
    // PHẦN 1: LOGIC NHẬP TÊN
    // ==========================================
    public void KiemTraTenHopLe()
    {
        CauHinhGame.TenNguoiChoi = oNhapTen.text.Trim();
        nutVaoGame.interactable = !string.IsNullOrEmpty(CauHinhGame.TenNguoiChoi);
    }

    // ==========================================
    // PHẦN 2: LOGIC MŨI TÊN CHỌN XE
    // ==========================================
    public void MuiTenPhai_ChonXe()
    {
        CauHinhGame.ChiSoXeChon++;
        if (CauHinhGame.ChiSoXeChon >= danhSachPrefabXe.Length)
        {
            CauHinhGame.ChiSoXeChon = 0;
        }
        CapNhatGiaoDienHinhAnh();
    }

    public void MuiTenTrai_ChonXe()
    {
        CauHinhGame.ChiSoXeChon--;
        if (CauHinhGame.ChiSoXeChon < 0)
        {
            CauHinhGame.ChiSoXeChon = danhSachPrefabXe.Length - 1;
        }
        CapNhatGiaoDienHinhAnh();
    }

    // ==========================================
    // PHẦN 3: LOGIC MŨI TÊN CHỌN ĐẠN
    // ==========================================
    public void MuiTenPhai_ChonDan()
    {
        CauHinhGame.ChiSoDanChon++;
        if (CauHinhGame.ChiSoDanChon >= mangHinhDan.Length)
        {
            CauHinhGame.ChiSoDanChon = 0;
        }
        CapNhatGiaoDienHinhAnh();
    }

    public void MuiTenTrai_ChonDan()
    {
        CauHinhGame.ChiSoDanChon--;
        if (CauHinhGame.ChiSoDanChon < 0)
        {
            CauHinhGame.ChiSoDanChon = mangHinhDan.Length - 1;
        }
        CapNhatGiaoDienHinhAnh();
    }

    // ==========================================
    // PHẦN 4: HIỂN THỊ VÀ CHUYỂN CẢNH
    // ==========================================
    private void CapNhatGiaoDienHinhAnh()
    {
        // ------------------------------------
        // 1. HIỂN THỊ XE BẰNG PREFAB THẬT
        // ------------------------------------
        // Xóa xe cũ nếu có
        if (xeDangDungTrongPhongChup != null)
        {
            Destroy(xeDangDungTrongPhongChup);
        }

        // Sinh ra chiếc xe mới
        if (danhSachPrefabXe.Length > 0 && diemDatXe != null)
        {
            xeDangDungTrongPhongChup = Instantiate(danhSachPrefabXe[CauHinhGame.ChiSoXeChon], diemDatXe.position, diemDatXe.rotation);

            // Tìm và xóa Script điều khiển để nó đứng im làm mẫu chụp ảnh
            TankController scriptDieuKhien = xeDangDungTrongPhongChup.GetComponent<TankController>();
            if (scriptDieuKhien != null)
            {
                Destroy(scriptDieuKhien);
            }
        }

        // ------------------------------------
        // 2. HIỂN THỊ ĐẠN BẰNG ẢNH 2D
        // ------------------------------------
        if (mangHinhDan.Length > 0 && imgHienThiDan != null)
        {
            imgHienThiDan.sprite = mangHinhDan[CauHinhGame.ChiSoDanChon];
        }
    }

    public void BatDauGame()
    {
        if (!string.IsNullOrEmpty(CauHinhGame.TenNguoiChoi))
        {
            SceneManager.LoadScene("Thai");
        }
    }
}