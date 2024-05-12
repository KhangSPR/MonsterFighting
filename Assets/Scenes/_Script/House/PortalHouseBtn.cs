using CodeMonkey.Utils;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("Set Portal");

        foreach (Portals portal in mapSO.portals)
        {
            // Set Portal
            if (portal.hasBoss)
            {
                _Electricity.gameObject.SetActive(portal.hasBoss);
                break;
            }

            Debug.Log("Electricity");

        }

        var mainModule = _Electricity.main;

        // Thay đổi thuộc tính startColor thông qua MainModule
        mainModule.startColor = GetColor(GetZone(mapSO.mapZone));

        var mainParticle = _Particle.main;

        mainParticle.startColor = GetColor(GetZone(mapSO.mapZone));

        //meterial
        _meterial.material = LevelUIManager.Instance.Materials[GetZone(mapSO.mapZone)];

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
