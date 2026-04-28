// [Vị trí: Assets/Scripts/Managers/QuanLyMenu.cs]
using UnityEngine;
using UnityEngine.UI; // Bắt buộc phải có cái này để dùng Image
using TMPro;
using UnityEngine.SceneManagement;

public class QuanLyMenu : MonoBehaviour
{
    [Header("Giao diện UI")]
    public TMP_InputField oNhapTen;
    public Button nutVaoGame;

    [Header("Khung Ảnh Hiển Thị")]
    public Image imgHienThiXe;   // Khung ảnh ở giữa 2 mũi tên chọn xe
    public Image imgHienThiDan;  // Khung ảnh ở giữa 2 mũi tên chọn đạn

    [Header("Kho Hình Ảnh (Kéo thả từ Project vào)")]
    public Sprite[] mangHinhXe;  // Nơi chứa 4 tấm hình xe của bạn
    public Sprite[] mangHinhDan; // Nơi chứa các tấm hình đạn

    void Start()
    {
        CauHinhGame.ChiSoXeChon = 0;
        CauHinhGame.ChiSoDanChon = 0;
        CapNhatGiaoDienHinhAnh();
        KiemTraTenHopLe();
    }

    public void KiemTraTenHopLe()
    {
        CauHinhGame.TenNguoiChoi = oNhapTen.text.Trim();
        nutVaoGame.interactable = !string.IsNullOrEmpty(CauHinhGame.TenNguoiChoi);
    }

    // --- MŨI TÊN XE ---
    public void MuiTenPhai_ChonXe()
    {
        CauHinhGame.ChiSoXeChon++;
        if (CauHinhGame.ChiSoXeChon >= mangHinhXe.Length) CauHinhGame.ChiSoXeChon = 0;
        CapNhatGiaoDienHinhAnh();
    }

    public void MuiTenTrai_ChonXe()
    {
        CauHinhGame.ChiSoXeChon--;
        if (CauHinhGame.ChiSoXeChon < 0) CauHinhGame.ChiSoXeChon = mangHinhXe.Length - 1;
        CapNhatGiaoDienHinhAnh();
    }

    // --- MŨI TÊN ĐẠN ---
    public void MuiTenPhai_ChonDan()
    {
        CauHinhGame.ChiSoDanChon++;
        if (CauHinhGame.ChiSoDanChon >= mangHinhDan.Length) CauHinhGame.ChiSoDanChon = 0;
        CapNhatGiaoDienHinhAnh();
    }

    public void MuiTenTrai_ChonDan()
    {
        CauHinhGame.ChiSoDanChon--;
        if (CauHinhGame.ChiSoDanChon < 0) CauHinhGame.ChiSoDanChon = mangHinhDan.Length - 1;
        CapNhatGiaoDienHinhAnh();
    }

    // --- LOGIC ĐỔI ẢNH (SPRITE) ---
    private void CapNhatGiaoDienHinhAnh()
    {
        // Kiểm tra xem mảng có hình không rồi mới gán ảnh vào khung
        if (mangHinhXe.Length > 0)
            imgHienThiXe.sprite = mangHinhXe[CauHinhGame.ChiSoXeChon];

        if (mangHinhDan.Length > 0)
            imgHienThiDan.sprite = mangHinhDan[CauHinhGame.ChiSoDanChon];
    }

    public void BatDauGame()
    {
        if (!string.IsNullOrEmpty(CauHinhGame.TenNguoiChoi))
        {
            SceneManager.LoadScene("Thai"); // Chuyển sang scene Thai
        }
    }
}