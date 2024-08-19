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

        public void SetLevelData(MapSO mapSO)
        {
            this.SetMapSo(mapSO);

            this.LoadItems(mapSO);
            this.LoadProtals(mapSO);
        }
        private void SetMapSo(MapSO MapSO)
        {
            mapSO = MapSO;
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
            foreach (Transform child in HolderItem)
            {
                Destroy(child.gameObject);
            }

            foreach (Resources resource in mapSO.Reward)
            {
                GameObject itemObject = Instantiate(ObjItem, HolderItem);

                //Set Resources
                itemObject.transform.Find("Img").GetComponent<Image>().sprite = gameMapIconSO.GetReWardIcon(resource.ItemReward.Type);
                itemObject.transform.Find("Count").GetComponent<Text>().text = "x" + resource.Count.ToString();

            }
        }
        public void OnButtonClickUIChosseMap()
        {
            // Gọi phương thức SetMapSOFromLevelInfo của UIChoosingMapLoader
            uIChoosingMap.SetMapSOFromLevelInfo(mapSO);

        }

    }
}
