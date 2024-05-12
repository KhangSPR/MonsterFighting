using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIGameDataMap
{
    public class MapCtrl : MonoBehaviour
    {
        [SerializeField] UIInGame uIInGame;
        public UIInGame UIInGame { get { return uIInGame; } }

        [SerializeField] PortalSpawnManager portalSpawnerCtrl;
        public PortalSpawnManager PortalSpawnerCtrl { get { return portalSpawnerCtrl; } }

        //[SerializeField] MapSO mapSO;
        //public MapSO MapSO { get { return mapSO; } set { mapSO = value; } }

    }
}
