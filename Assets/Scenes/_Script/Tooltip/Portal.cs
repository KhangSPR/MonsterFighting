using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public Portals portals;
    private void Start()
    {
        if (portals != null)
        {
            Tooltip_PortalsMap.AddTooltip(transform, portals);
        }
    }
    public void SetObjPortal(Portals portal)
    {
        ParticleSystem _Electricity = transform.Find("ElectricityBoss").GetComponent<ParticleSystem>();
        var mainModule = _Electricity.main;

        // Thay ??i thu?c tính startColor thông qua MainModule
        mainModule.startColor = portal.GetColorForRarityPortal(portal.rarityPortal);

        ParticleSystem _Particle = transform.Find("Particle").GetComponent<ParticleSystem>();

        var mainParticle = _Particle.main;

        mainParticle.startColor = portal.GetColorForRarityPortal(portal.rarityPortal);

        Image _meterial = transform.Find("Circle").GetComponent<Image>();
        _meterial.material = LevelUIManager.Instance.Materials[portal.GetIndexRarity(portal.rarityPortal)];
    }
}
