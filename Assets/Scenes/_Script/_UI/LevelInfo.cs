using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace UIGameDataMap
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField] private GameMapIconSO gameMapIconSO;
        [SerializeField] Text ZoneIndex;
        [SerializeField] UIChoosingMapLoader uIChoosingMap;

        [Header("Portals")]
        [SerializeField] Transform HolderPortals;
        [SerializeField] GameObject objPortalTooltip;
        [SerializeField] Window_Portals window_Portals;

        [Header("Item")]
        [SerializeField] Transform HolderItem;
        [SerializeField] GameObject ObjItem;

        [Header("MapSO")]
        [SerializeField] MapSO mapSO;
        public MapSO MapSO { get { return mapSO; } }

        [Header("DifficultMap")]
        [SerializeField] ChangeDifficultMapInfos MapInfos;
        private void SetChangeDifficultMapInfos(MapSO mapSO)
        {
            foreach (ChangeDifficultMap mapInfo in MapInfos.ChangeDifficultMapInfo)
            {
                mapInfo.SetMapSO(mapSO);
            }
        }
        public void SetLevelDataDifficulty(MapSO mapSO, MapDifficulty mapDifficulty)
        {

            if (mapDifficulty == null)
            {
                mapDifficulty = mapSO.DifficultyMap[0];
            }
            this.SetMapSo(mapSO);

            this.LoadItems(mapDifficulty);
            this.LoadPortals(mapDifficulty);
            this.SetChangeDifficultMapInfos(mapSO);
        }
        private void SetMapSo(MapSO MapSO)
        {
            mapSO = MapSO;
        }
        public void LoadPortals(MapDifficulty mapDifficulty)
        {
            // Lấy các wave từ MapSO theo độ khó
            Wave[] waves = mapSO.GetWaves(mapDifficulty.difficult);

            // Khởi tạo từ điển để gộp các portal theo rarity
            Dictionary<RarityPortal, Portals> portalsDict = new Dictionary<RarityPortal, Portals>();

            // Duyệt qua từng wave để lấy portals và thêm vào từ điển
            foreach (var wave in waves)
            {
                Portals[] portalsFromWave = mapSO.GetPortalsWave(wave);
                if (portalsFromWave != null)
                {
                    foreach (var portal in portalsFromWave)
                    {
                        AddOrUpdatePortal(portalsDict, portal);
                    }
                }
            }

            foreach (var wave in waves)
            {
                Portals[] portalsFromWave = mapSO.GetPortalsSpawning(wave);
                if (portalsFromWave != null)
                {
                    foreach (var portal in portalsFromWave)
                    {
                        AddOrUpdatePortal(portalsDict, portal);
                    }
                }
            }

            // Nếu danh sách portals trống thì thoát hàm
            if (portalsDict.Count == 0)
            {
                Debug.LogWarning("No portals found for the given waves.");
                return;
            }

            // Xóa tất cả các Portal hiện có trong HolderPortals
            foreach (Transform child in HolderPortals)
            {
                Destroy(child.gameObject);
            }

            int index = 0; // Khai báo biến index ở đây để giữ giá trị của nó sau mỗi lần lặp

            // Duyệt qua từng portal trong từ điển
            foreach (var portalEntry in portalsDict.Values)
            {
                // Instantiate object cho mỗi portal
                GameObject portalObject = Instantiate(objPortalTooltip, HolderPortals);

                // Set các giá trị cho Portal object
                Portal portalComponent = portalObject.GetComponent<Portal>();

                portalComponent.portals = portalEntry;
                portalComponent.SetObjPortal(portalEntry); // Set Portal

                // Set số lượng portal hiển thị trong "Count"
                portalObject.transform.Find("Count").GetComponent<Text>().text = "x" + portalEntry.countPortal.ToString();

                // Hiển thị hoặc ẩn ElectricityBoss nếu có boss
                portalObject.transform.Find("ElectricityBoss").gameObject.SetActive(portalEntry.hasBoss);

                index++; // Tăng giá trị index sau mỗi lần lặp
            }
        }

        private void AddOrUpdatePortal(Dictionary<RarityPortal, Portals> portalsDict, Portals portal)
        {
            if (portal == null)
                return;

            if (portalsDict.ContainsKey(portal.rarityPortal))
            {
                // Nếu đã tồn tại, tăng countPortal
                portalsDict[portal.rarityPortal].countPortal += portal.countPortal;

                // Gộp danh sách enemyTypes mà không trùng lặp và cộng dồn countEnemy nếu trùng tên
                var currentEnemyTypes = portalsDict[portal.rarityPortal].enemyTypes.ToList();
                foreach (var newEnemyType in portal.enemyTypes)
                {
                    if (newEnemyType.enemyTypeSO == null)
                        continue; // Bỏ qua nếu newEnemyType không hợp lệ

                    var existingEnemyType = currentEnemyTypes.FirstOrDefault(e =>
                        e.enemyTypeSO != null && // Kiểm tra null trước khi so sánh
                        e.enemyTypeSO.enemyName == newEnemyType.enemyTypeSO.enemyName);

                    if (existingEnemyType != null)
                    {
                        // Cộng dồn số lượng enemy nếu trùng tên
                        existingEnemyType.countEnemy += newEnemyType.countEnemy;
                    }
                    else
                    {
                        // Thêm enemy mới vào danh sách
                        currentEnemyTypes.Add(newEnemyType);
                    }
                }

                portalsDict[portal.rarityPortal].enemyTypes = currentEnemyTypes.ToArray(); // Cập nhật danh sách enemyTypes
            }
            else
            {
                // Nếu chưa tồn tại, thêm portal vào từ điển
                portalsDict[portal.rarityPortal] = new Portals
                {
                    rarityPortal = portal.rarityPortal,
                    countPortal = portal.countPortal,
                    hasBoss = portal.hasBoss,
                    enemyTypes = portal.enemyTypes.ToArray() // Sao chép mảng
                };
            }
        }





        private void LoadItems(MapDifficulty mapDifficulty)
        {

            foreach (Transform child in HolderItem)
            {
                Destroy(child.gameObject);
            }

            foreach (Resources resource in mapDifficulty.Reward)
            {

                GameObject itemObject = Instantiate(ObjItem, HolderItem);
                if (mapDifficulty.isReceivedReWard)
                {
                    //Set Resources
                    itemObject.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;

                    itemObject.transform.Find("Img").GetComponent<Image>().color = new Color(1f, 160 / 255f, 122 / 255f, 128 / 255f);


                    itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

                    itemObject.transform.Find("Count").GetComponent<Text>().color = new Color(1f, 1f, 1f, 128 / 255f);
                }
                else
                {
                    //Set Resources
                    itemObject.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;
                    itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();
                }

            }
        }
        public void OnButtonClickUIChosseMap()
        {
            // Gọi phương thức SetMapSOFromLevelInfo của UIChoosingMapLoader
            uIChoosingMap.SetMapSOFromLevelInfo(mapSO);

        }

    }
}
