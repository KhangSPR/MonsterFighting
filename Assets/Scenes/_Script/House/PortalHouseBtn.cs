using CodeMonkey.Utils;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class PortalHouseBtn : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField] ParticleSystem _Electricity;
    [SerializeField] ParticleSystem _Particle;
    [SerializeField] SpriteRenderer _meterial;

    [SerializeField] Button_UI button_UI;
    [SerializeField] BtnUI btnUI;
    private void Start()
    {
        button_UI.MouseUpFunc = () => btnUI.OnClickButton();

    }
    public void SetPortals(MapSO mapSO)
    {
        // Kiểm tra xem mapSO có null không
        if (mapSO == null)
        {
            Debug.LogError("MapSO is null");
            return;
        }

        Wave[] waves = mapSO.GetWaves(MapManager.Instance.Difficult);

        if (waves == null)
        {
            Debug.Log("No Wave found.");
            return;
        }

        // Khởi tạo danh sách portals
        List<Portals> portals = new List<Portals>();

        // Duyệt qua từng wave để lấy portals và thêm vào danh sách
        foreach (var wave in waves)
        {
            Portals[] portalsFromWave = mapSO.GetPortalsWave(wave);
            if (portalsFromWave != null && portalsFromWave.Length > 0)
            {
                portals.AddRange(portalsFromWave); // Thêm tất cả portals vào danh sách
            }
        }

        // Kiểm tra nếu danh sách portals trống
        if (portals == null || portals.Count == 0)
        {
            Debug.LogError("No portals found for the given waves.");
            return;
        }
        //Debug.Log("CountPortal: " + portals.Count);

        // Duyệt qua các portal và kích hoạt Electricity nếu có boss
        foreach (Portals portal in portals)
        {
            if (portal.hasBoss)
            {
                _Electricity.gameObject.SetActive(true);
                break; // Dừng vòng lặp khi đã tìm thấy boss
            }
        }

        // Kiểm tra các thành phần cần thiết có null không
        if (_Electricity == null || _Particle == null || _meterial == null)
        {
            Debug.LogError("One or more required components (_Electricity, _Particle, or _meterial) are null");
            return;
        }

        // Thiết lập màu sắc cho _Electricity và _Particle dựa trên zone của map
        var mainModule = _Electricity.main;
        mainModule.startColor = GetColor(GetZone(mapSO.mapZone));

        var mainParticle = _Particle.main;
        mainParticle.startColor = GetColor(GetZone(mapSO.mapZone));

        // Kiểm tra nếu materials của LevelUIManager hợp lệ
        int zoneIndex = GetZone(mapSO.mapZone);
        if (LevelUIManager.Instance.Materials == null || zoneIndex >= LevelUIManager.Instance.Materials.Length)
        {
            Debug.LogError("Materials array is null or zone index is out of bounds.");
            return;
        }

        // Thiết lập material cho _meterial dựa trên zone
        _meterial.material = LevelUIManager.Instance.Materials[zoneIndex];
    }


    public Color GetColor(int level)
    {
        switch (level)
        {
            case 0:
                return Color.white; // Màu trắng cho Common
            case 1:
                return new Color(0.164f, 1f, 0.898f); // Color Code "2AFFE5"
            case 2:
                return new Color(255f / 255f, 18f / 255f, 128f / 255f);
            case 3:
                return Color.yellow; // Màu vàng cho Legendary
            default:
                return Color.gray; // Mặc định màu xám
        }
    }
    public int GetZone(string zone)
    {
        switch (zone)
        {
            case "I":
                return 0;
            case "II":
                return 1;
            case "III":
                return 2;
            case "IV":
                return 3;
            default:
                return 0;
        }
    }
}
