using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] MapSO mapSO;
    public MapSO MapSO { get { return mapSO; } set { mapSO = value; } }

    [SerializeField] int portalsIndex;

    public int PortalsIndex { get { return portalsIndex; } set { portalsIndex = value; } }
    private void Start()
    {
        if (mapSO != null)
        {
            Tooltip_PortalsMap.AddTooltip(transform, mapSO, portalsIndex);
        }
    }
    public void SetObjPortal(Portals portal)
    {
        ParticleSystem _Electricity = transform.Find("ElectricityBoss").GetComponent<ParticleSystem>();
        var mainModule = _Electricity.main;

        // Thay ??i thu?c tính startColor thông qua MainModule
        mainModule.startColor = mapSO.GetColorForRarityPortal(portal.rarityPortal);

        ParticleSystem _Particle = transform.Find("Particle").GetComponent<ParticleSystem>();

        var mainParticle = _Particle.main;

        mainParticle.startColor = mapSO.GetColorForRarityPortal(portal.rarityPortal);

        Image _meterial = transform.Find("Circle").GetComponent<Image>();
        _meterial.material = LevelUIManager.Instance.Materials[mapSO.GetIndexRarity(portal.rarityPortal)];
    }
}
