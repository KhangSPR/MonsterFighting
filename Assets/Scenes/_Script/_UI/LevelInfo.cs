using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UIGameDataMap
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField] private GameMapIconSO gameMapIconSO;
        [SerializeField] private Text zoneIndex;
        [SerializeField] private UIChoosingMapLoader uIChoosingMap;

        [Header("Portals")]
        [SerializeField] private Transform holderPortals;
        [SerializeField] private GameObject portalTooltipPrefab;
        [SerializeField] private Window_Portals windowPortals;

        [Header("Item")]
        [SerializeField] private Transform holderItem;
        [SerializeField] private GameObject itemPrefab;

        [Header("MapSO")]
        [SerializeField] private MapSO mapSO;
        public MapSO MapSO => mapSO;

        [Header("DifficultMap")]
        [SerializeField] private ChangeDifficultMapInfos mapInfos;

        private void SetChangeDifficultMapInfos(MapSO mapSO)
        {
            foreach (var mapInfo in mapInfos.ChangeDifficultMapInfo)
            {
                mapInfo.SetMapSO(mapSO);
            }
        }

        public void SetLevelDataDifficulty(MapSO mapSO, MapDifficulty mapDifficulty)
        {
            mapDifficulty ??= mapSO.DifficultyMap.FirstOrDefault();

            SetMapSO(mapSO);
            LoadItems(mapDifficulty);
            LoadPortals(mapDifficulty);
            SetChangeDifficultMapInfos(mapSO);
        }

        private void SetMapSO(MapSO mapSO)
        {
            this.mapSO = mapSO;
        }

        public void LoadPortals(MapDifficulty mapDifficulty)
        {
            var waves = mapSO.GetWaves(mapDifficulty.difficult);
            var portalsDict = new Dictionary<RarityPortal, Portals>();

            foreach (var wave in waves)
            {
                MergePortalsIntoDictionary(portalsDict, mapSO.GetPortalsWave(wave));
                MergePortalsIntoDictionary(portalsDict, mapSO.GetPortalsSpawning(wave));
            }

            foreach (Transform child in holderPortals)
            {
                Destroy(child.gameObject);
            }

            if (!portalsDict.Any()) return;

            foreach (var portalEntry in portalsDict.Values)
            {
                var portalObject = Instantiate(portalTooltipPrefab, holderPortals);
                var portalComponent = portalObject.GetComponent<Portal>();

                portalComponent.portals = portalEntry;
                portalComponent.SetObjPortal(portalEntry);

                portalObject.transform.Find("Count").GetComponent<Text>().text = "x" + portalEntry.countPortal;
                portalObject.transform.Find("ElectricityBoss").gameObject.SetActive(portalEntry.hasBoss);
            }
        }

        private void MergePortalsIntoDictionary(Dictionary<RarityPortal, Portals> portalsDict, Portals[] portals)
        {
            if (portals == null) return;

            foreach (var portal in portals)
            {
                if (portal == null) continue;

                if (portalsDict.TryGetValue(portal.rarityPortal, out var existingPortal))
                {
                    existingPortal.countPortal += portal.countPortal;

                    foreach (var newEnemyType in portal.enemyTypes)
                    {
                        if (newEnemyType.enemyTypeSO == null) continue;

                        var existingEnemyType = existingPortal.enemyTypes.FirstOrDefault(e => e.enemyTypeSO?.enemyName == newEnemyType.enemyTypeSO.enemyName);

                        if (existingEnemyType != null)
                        {
                            existingEnemyType.countEnemy += newEnemyType.countEnemy;
                        }
                        else
                        {
                            var enemyTypesList = existingPortal.enemyTypes.ToList();
                            enemyTypesList.Add(newEnemyType);
                            existingPortal.enemyTypes = enemyTypesList.ToArray();
                        }
                    }
                }
                else
                {
                    portalsDict[portal.rarityPortal] = new Portals
                    {
                        rarityPortal = portal.rarityPortal,
                        countPortal = portal.countPortal,
                        hasBoss = portal.hasBoss,
                        enemyTypes = portal.enemyTypes.ToArray()
                    };
                }
            }
        }

        private void LoadItems(MapDifficulty mapDifficulty)
        {
            foreach (Transform child in holderItem)
            {
                Destroy(child.gameObject);
            }

            foreach (Resources resource in mapDifficulty.Reward)
            {
                GameObject itemObject = Instantiate(itemPrefab, holderItem);
                Image imageComponent = itemObject.transform.Find("Img").GetComponent<Image>();
                ItemTooltipReward itemTooltip = itemObject.GetComponent<ItemTooltipReward>();


                itemTooltip.ItemReward = resource.item;
                imageComponent.sprite = resource.item.Image;
                itemTooltip.CountTxt.text = "x" + resource.Count;

                if (mapDifficulty.isReceivedReWard)
                {
                    imageComponent.color = new Color(1f, 160 / 255f, 122 / 255f, 128 / 255f);
                    itemTooltip.CountTxt.color = new Color(1f, 1f, 1f, 128 / 255f);

                    itemObject.transform.Find("Tick").gameObject.SetActive(true);
                }
            }
        }

        public void OnButtonClickUIChooseMap()
        {
            uIChoosingMap.SetMapSOFromLevelInfo(mapSO);
        }
    }
}
