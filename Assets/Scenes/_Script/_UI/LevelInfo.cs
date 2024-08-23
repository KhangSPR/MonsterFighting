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
        public MapSO MapSO { get { return mapSO; }  }

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

            this.LoadItems(mapSO);
            this.LoadProtals(mapDifficulty);
            this.SetChangeDifficultMapInfos(mapSO);
        }      
        private void SetMapSo(MapSO MapSO)
        {
            mapSO = MapSO;
        }
        public void LoadProtals(MapDifficulty mapDifficulty)
        {
            Portals[] portals = mapSO.GetPortals(mapDifficulty.difficult);
            foreach (Transform child in HolderPortals)
            {
                Destroy(child.gameObject);
            }

            int index = 0; // Khai báo biến index ở đây để giữ giá trị của nó sau mỗi lần lặp

            foreach (Portals portal in portals)
            {

                GameObject portalObject = Instantiate(objPortalTooltip, HolderPortals);


                portalObject.GetComponent<Portal>().MapSO = mapSO; // Set MapSo
                portalObject.GetComponent<Portal>().MapDifficulty = mapDifficulty;
                portalObject.GetComponent<Portal>().PortalsIndex = index; // Set Index ToolTip
                portalObject.GetComponent<Portal>().SetObjPortal(portal);
                // Set Portal
                portalObject.transform.Find("Count").GetComponent<Text>().text = "x" + portal.Count.ToString();
                portalObject.transform.Find("ElectricityBoss").gameObject.SetActive(portal.hasBoss); //Electricity Boss

                index++;

            }

        }
        private void LoadItems(MapSO mapSO)
        {

            foreach (Transform child in HolderItem)
            {
                Destroy(child.gameObject);
            }

            //foreach (Resources resource in mapSO.Reward)
            //{

            //    GameObject itemObject = Instantiate(ObjItem, HolderItem);
            //    //if (mapSO.isReceived)
            //    //{
            //    //    //Set Resources
            //    //    itemObject.transform.Find("Img").GetComponent<Image>().sprite = gameMapIconSO.GetReWardIcon(resource.ItemReward.Type);

            //    //    itemObject.transform.Find("Img").GetComponent<Image>().color = new Color(1f, 160 / 255f, 122 / 255f, 128 / 255f);


            //    //    itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

            //    //    itemObject.transform.Find("Count").GetComponent<Text>().color = new Color(1f, 1f, 1f, 128 / 255f);
            //    //}
            //    //else
            //    //{
            //    //    //Set Resources
            //    //    itemObject.transform.Find("Img").GetComponent<Image>().sprite = gameMapIconSO.GetReWardIcon(resource.ItemReward.Type);
            //    //    itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();
            //    //}

            //}
            //switch (mapSO.difficult)
            //{
            //    case Difficult.Easy:
            //        {
            //            foreach (Resources resource in mapSO.RewardEasy)
            //            {
            //                //Debug.Log($"mapSO :{resource}");
            //                GameObject itemObject = Instantiate(ObjItem, HolderItem);

            //                //Set Resources
            //                itemObject.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;//gameMapIconSO.GetReWardIcon(resource.item);
            //                Debug.Log("Reward Icon", resource.item.Image);
            //                itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

            //            }
            //        }
            //        break;
            //    case Difficult.Normal:
            //        {
            //            foreach (Resources resource in mapSO.RewardNormal)
            //            {
            //                Debug.Log($"mapSO :{resource}");
            //                GameObject itemObject = Instantiate(ObjItem, HolderItem);

            //                //Set Resources
            //                itemObject.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;//gameMapIconSO.GetReWardIcon(resource.item);
            //                Debug.Log("Reward Icon", resource.item.Image);
            //                itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

            //            }
            //        }
            //        break;
            //    case Difficult.Hard:
            //        {
            //            foreach (Resources resource in mapSO.RewardHard)
            //            {
            //                Debug.Log($"mapSO :{resource}");
            //                GameObject itemObject = Instantiate(ObjItem, HolderItem);

            //                //Set Resources
            //                itemObject.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;//gameMapIconSO.GetReWardIcon(resource.item);
            //                Debug.Log("Reward Icon", resource.item.Image);
            //                itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

            //            }
            //        }
            //        break;
            //}
        }
        public void OnButtonClickUIChosseMap()
        {
            // Gọi phương thức SetMapSOFromLevelInfo của UIChoosingMapLoader
            uIChoosingMap.SetMapSOFromLevelInfo(mapSO);

        }

    }
}
