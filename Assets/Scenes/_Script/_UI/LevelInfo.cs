using System.Collections.Generic;
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
            foreach (ChangeDifficultMapInfo mapInfo in MapInfos.ChangeDifficultMapInfo)
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

            // Khởi tạo danh sách portals
            List<Portals> portalsList = new List<Portals>();

            // Duyệt qua từng wave để lấy portals và thêm vào danh sách
            foreach (var wave in waves)
            {
                Portals[] portalsFromWave = mapSO.GetPortalsWave(wave);
                if (portalsFromWave != null)
                {
                    portalsList.AddRange(portalsFromWave); // Thêm tất cả portals vào danh sách
                }
            }

            Debug.Log("portalsList: " + portalsList.Count);

            // Nếu danh sách portals trống thì thoát hàm
            if (portalsList.Count == 0)
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

            // Duyệt qua từng portal trong danh sách portals
            foreach (Portals portal in portalsList)
            {
                // Instantiate object cho mỗi portal
                GameObject portalObject = Instantiate(objPortalTooltip, HolderPortals);

                // Set các giá trị cho Portal object
                Portal portalComponent = portalObject.GetComponent<Portal>();
                portalComponent.MapSO = mapSO; // Set MapSO
                portalComponent.MapDifficulty = mapDifficulty; // Set MapDifficulty
                portalComponent.PortalsIndex = index; // Set Index ToolTip
                portalComponent.SetObjPortal(portal); // Set Portal

                // Set số lượng portal hiển thị trong "Count"
                portalObject.transform.Find("Count").GetComponent<Text>().text = "x" + portal.countPortal.ToString();

                // Hiển thị hoặc ẩn ElectricityBoss nếu có boss
                portalObject.transform.Find("ElectricityBoss").gameObject.SetActive(portal.hasBoss);

                index++; // Tăng giá trị index sau mỗi lần lặp
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
