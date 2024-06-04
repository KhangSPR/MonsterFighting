using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
namespace UIGameDataMap
{
    public class LevelButton : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] private MapSO mapDataSO; //Script able Object

        [SerializeField] private LevelInfo levelInfo; // Reference đến script LevelObject

        [Header("GameObject")]
        [SerializeField] private GameObject lockObj;
        [SerializeField] private GameObject unlockObj;     //ref to lock and unlock gameobject
        [SerializeField] private GameObject activeLevelIndicator;
        [SerializeField] private GameObject ObjectAttack;
        [SerializeField] private GameObject DifficultHolderUI;

        [Header("Level Settings")]
        [SerializeField] private Text ZoneIndexText;               // ref to text which indicates the level number
        [SerializeField] private Button btn;
        /*[SerializeField] private Image[] starsArray;  */              //ref to all the stars of button
                                                                        //ref to hold button component
        //public static event Action OnClickEnvent;
        public MapSO GetMapSO()
        {
            return mapDataSO;
        }
        private int levelIndex;                                     //int which hold the level Index this perticular button specify

        private void Start()
        {
            SetLevelButton(GetAreaName());
            btn.onClick.AddListener(() => OnClick());               //add listener to the button

        }
        public MapSO GetMapDataSO()
        {
            return mapDataSO;
        }
        int GetLevelIndex()
        {
            //Debug.Log(transform.GetSiblingIndex() - 1 + "transform.GetSiblingIndex() - 1");
            return transform.GetSiblingIndex() - 1;
        }
        string GetAreaName()
        {
            return transform.parent.parent.name;
        }
        void SetUnlockedUI()
        {
            lockObj.SetActive(false);                           //deactivate lockObj
            unlockObj.SetActive(true);                          //activate unlockObj
            activeLevelIndicator.SetActive(false);
        }
        void SetLockedUI()
        {
            lockObj.SetActive(true);                           //deactivate lockObj
            unlockObj.SetActive(false);                          //activate unlockObj
            activeLevelIndicator.SetActive(true);
        }

        public void SetLevelButton(string areaName)
        {
            int index = GetLevelIndex();
            Debug.Log("index level:" + index);
            AreaInfomationSO aiso = LevelSystemManager.Instance.aiso;
            //Debug.Log(aiso);
            var levelInformation = aiso.areasData.First(data => data.areaName == areaName);

            //Debug.Log("SetLevelButton " + levelInformation);
            if (levelInformation.levelsData[index].isUnlocked)
            {
                SetUnlockedUI();
                //Set LevelInfo
                levelInfo = LevelUIManager.Instance.LevelInfo;
                //Set button
                mapDataSO = LevelUIManager.Instance.GetMapSO(index, GetTyMap(areaName));
                
                ZoneIndexText.text = mapDataSO.mapZone;
                //SetAuto Click
                if (LevelUIManager.Instance.CurrentLevelButton == this)
                {
                    OnClick();
                }
                else
                    Debug.Log("Khong phai LV Can Lay");
            }
            else
            {
                SetLockedUI();
                ZoneIndexText.text = "";
            }
            /*if (value.unlocked)                                     //if unlocked is true
            {
                //activeLevelIndicator.SetActive(activeLevel);
                levelIndex = index;                             //set levelIndex, Note: We add 1 because array start from 0 and level index start from 1 
                btn.interactable = true;                            //make button interactable
                
                *//*SetStar(value.starAchieved);*//*                     //set the stars
                ZoneIndexText.text = "" + mapSO.mapZone;              //set levelIndexText text
                this.mapDataSO = mapSO;
            }
            else
            {
                activeLevelIndicator.SetActive(true);
                btn.interactable = false;                           //remove button interactable
                lockObj.SetActive(true);                            //activate lockObj
                unlockObj.SetActive(false);                         //deactivate unlockObj
            }*/
        }
        public MapType GetTyMap(string namemap)
        {
            switch (namemap)
            {
                case "GrassChoosen":
                    return MapType.Grass;
                case "LavaChoosen":
                    return MapType.Lava;
                default:
                    return MapType.Grass;
            }
        }
        public int GetLVInfo(string namemap)
        {
            switch (namemap)
            {
                case "GrassChoosen":
                    return 0;
                case "LavaChoosen":
                    return 1;
                default:
                    return 0;
            }
        }
        public Difficult GetDifficult(int difficult)
        {
            switch (difficult)
            {
                case 1:
                    return Difficult.Easy;
                case 2:
                    return Difficult.Normal;
                case 3:
                    return Difficult.Hard;
                default:
                    return Difficult.Easy;
            }
        }
        public void OnClick()                                              //method called by button
        {
            /*LevelSystemManager.Instance.CurrentLevel = levelIndex;*/  //set the CurrentLevel, we subtract 1 as level data array start from 0
            if(levelInfo==null)
            {
                return;
            }
            levelInfo.SetLevelData(mapDataSO);
            levelInfo.OnButtonClickUIChosseMap();
            Debug.Log("Diffcult HOlder = "+ transform.parent.parent.parent.parent.parent.parent.Find("DifficultHolder"));
            DifficultHolderUI = transform.parent.parent.parent.parent.parent.parent.Find("DifficultHolder").gameObject;
            if (DifficultHolderUI.activeSelf)
            {
                DifficultHolderUI.transform.GetChild(1).gameObject.SetActive(mapDataSO.starsEasy == 3);
                DifficultHolderUI.transform.GetChild(2).gameObject.SetActive(mapDataSO.starsNormal == 3);
            }
            //Onclick 1 
            //OnClickEnvent.Invoke();

            Debug.Log("Click");


            // Add ObjectAttack to the ObjectAttackManager
            if (ObjectAttack != null)
            {
                ObjectAttackManager.Instance.AddObjectToManager(ObjectAttack);
            }
            //SceneManager.LoadScene("Level" + levelIndex);           //load the level
        }
        //private void SetStar(int starAchieved)
        //{
        //    for (int i = 0; i < starsArray.Length; i++)             //loop through entire star array
        //    {
        //        /// <summary>
        //        /// if i is less than starAchieved
        //        /// Eg: if 2 stars are achieved we set the start at index 0 and 1 color to unlockColor, as array start from 0 element
        //        /// </summary>
        //        if (i < starAchieved)
        //        {
        //            starsArray[i].color = unlockColor;              //set its color to unlockColor
        //        }
        //        else
        //        {
        //            starsArray[i].color = lockColor;                //else set its color to lockColor
        //        }
        //    }
        //}

    }
}