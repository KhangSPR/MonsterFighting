using System.Collections.Generic;
using UIGameDataManager;
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
        [SerializeField] Transform HolderItemOneTime;
        [SerializeField] GameObject ObjItem;

        [Header("MapSO")]
        [SerializeField] MapSO mapSO;
        public MapSO MapSO { get { return mapSO; } }

        public void SetLevelData(MapSO mapSO)
        {
            this.SetMapSo(mapSO);

            this.LoadItems(mapSO);
            this.LoadProtals(mapSO);
        }
        private void SetMapSo(MapSO MapSO)
        {
            mapSO = MapSO;
            GameDataManager.Instance.currentMapSO = mapSO;
        }

        private void LoadProtals(MapSO mapSO)
        {
            foreach (Transform child in HolderPortals)
            {
                Destroy(child.gameObject);
            }

            int index = 0; // Khai báo biến index ở đây để giữ giá trị của nó sau mỗi lần lặp

            foreach (Portals portal in mapSO.portals)
            {

                GameObject portalObject = Instantiate(objPortalTooltip, HolderPortals);


                portalObject.GetComponent<Portal>().MapSO = mapSO; // Set MapSo
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
            Debug.Log($"Holder Item Child Count :<b>{HolderItem.childCount}</b>");
            foreach (Transform child in HolderItem)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in HolderItemOneTime)
            {
                Destroy(child.gameObject);
            }
            foreach (Resources resource in mapSO.Reward)
            {
                Debug.Log($"mapSO :{resource}");
                GameObject itemObject = Instantiate(ObjItem, HolderItem);

                //Set Resources
                itemObject.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;//gameMapIconSO.GetReWardIcon(resource.item);
                Debug.Log("Reward Icon", resource.item.Image);
                itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

            }
            foreach (Resources resource in mapSO.OneTimeReward)
            {
                Debug.Log("Item : " + resource);
                GameObject itemObject = Instantiate(ObjItem, HolderItemOneTime);
                itemObject.transform.Find("Img").GetComponent<Image>().sprite = !mapSO.isOneTimeRewardGot? resource.item.Image: resource.item.ImageBnW;
                itemObject.transform.Find("Count").GetComponent<Text>().text = resource.Count.ToString();
            }
        }
        public void OnButtonClickUIChosseMap()
        {
            // Gọi phương thức SetMapSOFromLevelInfo của UIChoosingMapLoader
            uIChoosingMap.SetMapSOFromLevelInfo(mapSO);

        }

    }
}
