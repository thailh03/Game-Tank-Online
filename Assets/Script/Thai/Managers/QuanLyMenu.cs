// [Vị trí: Assets/Scripts/Thai/Managers/QuanLyMenu.cs]
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuanLyMenu : MonoBehaviour
{
    [Header("--- GIAO DIỆN NHẬP LIỆU ---")]
    public TMP_InputField oNhapTen;
    public Button nutVaoGame;

    [Header("--- STUDIO 1: CHỤP XE TANK ---")]
    public GameObject[] danhSachPrefabXe;
    public Transform diemDatXe;
    private GameObject xeDangDungTrongPhongChup;

    [Header("--- STUDIO 2: CHỤP VIÊN ĐẠN ---")]
    public GameObject[] danhSachPrefabDan;
    public Transform diemDatDan;
    private GameObject danDangDungTrongPhongChup;

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

    // --- LOGIC MŨI TÊN CHỌN XE ---
    public void MuiTenPhai_ChonXe()
    {
        CauHinhGame.ChiSoXeChon++;
        if (CauHinhGame.ChiSoXeChon >= danhSachPrefabXe.Length) CauHinhGame.ChiSoXeChon = 0;
        CapNhatGiaoDienHinhAnh();
    }

    public void MuiTenTrai_ChonXe()
    {
        CauHinhGame.ChiSoXeChon--;
        if (CauHinhGame.ChiSoXeChon < 0) CauHinhGame.ChiSoXeChon = danhSachPrefabXe.Length - 1;
        CapNhatGiaoDienHinhAnh();
    }

    // --- LOGIC MŨI TÊN CHỌN ĐẠN ---
    public void MuiTenPhai_ChonDan()
    {
        CauHinhGame.ChiSoDanChon++;
        if (CauHinhGame.ChiSoDanChon >= danhSachPrefabDan.Length) CauHinhGame.ChiSoDanChon = 0;
        CapNhatGiaoDienHinhAnh();
    }

    public void MuiTenTrai_ChonDan()
    {
        CauHinhGame.ChiSoDanChon--;
        if (CauHinhGame.ChiSoDanChon < 0) CauHinhGame.ChiSoDanChon = danhSachPrefabDan.Length - 1;
        CapNhatGiaoDienHinhAnh();
    }

    // --- LOGIC XUẤT HÌNH RA CAMERA ---
    private void CapNhatGiaoDienHinhAnh()
    {
        // 1. Sinh xe mới ở Studio 1
        if (xeDangDungTrongPhongChup != null) Destroy(xeDangDungTrongPhongChup);
        if (danhSachPrefabXe.Length > 0 && diemDatXe != null)
        {
            xeDangDungTrongPhongChup = Instantiate(danhSachPrefabXe[CauHinhGame.ChiSoXeChon], diemDatXe.position, diemDatXe.rotation);
            XoaLinhTinh(xeDangDungTrongPhongChup);
        }

        // 2. Sinh đạn mới ở Studio 2
        if (danDangDungTrongPhongChup != null) Destroy(danDangDungTrongPhongChup);
        if (danhSachPrefabDan.Length > 0 && diemDatDan != null)
        {
            danDangDungTrongPhongChup = Instantiate(danhSachPrefabDan[CauHinhGame.ChiSoDanChon], diemDatDan.position, diemDatDan.rotation);
            XoaLinhTinh(danDangDungTrongPhongChup);
        }
    }

    // Hàm để dọn dẹp các script giúp xe/đạn đứng im làm mẫu
    // Hàm để dọn dẹp, biến cỗ máy chiến đấu thành bức tượng vô hại
    private void XoaLinhTinh(GameObject obj)
    {
        // 1. Xóa vật lý (Để xe/đạn không bị rơi tự do hoặc va chạm lung tung)
        if (obj.GetComponent<Rigidbody2D>()) Destroy(obj.GetComponent<Rigidbody2D>());
        if (obj.GetComponent<Collider2D>()) Destroy(obj.GetComponent<Collider2D>());

        // 2. GÂY MÊ TUYỆT ĐỐI: Quét và tắt sạch toàn bộ các Script tự viết
        // Lệnh này sẽ lùng sục từ thân xe đến nòng súng, có Script nào là tắt hết
        MonoBehaviour[] tatCaCacScript = obj.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in tatCaCacScript)
        {
            // Ngoại trừ chính cái script QuanLyMenu này (nếu lỡ quét trúng)
            if (script != this)
            {
                script.enabled = false; // Tắt hoàn toàn hoạt động của script
            }
        }
    }

    public void BatDauGame()
    {
        if (!string.IsNullOrEmpty(CauHinhGame.TenNguoiChoi)) SceneManager.LoadScene("Thai");
    }
}