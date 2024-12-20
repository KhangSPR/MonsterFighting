using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIGameDataMap
{
    public class LevelButton : MonoBehaviour
    {
        [Header("Scripts & Data")]
        [SerializeField] private MapSO mapDataSO; // Scriptable Object lưu trữ dữ liệu bản đồ
        public MapSO MapSO => mapDataSO;
        [SerializeField] private LevelInfo levelInfo; // Script tham chiếu đến thông tin level
        [SerializeField] private ChangeDifficultMapInfos changeDifficultMapInfos; // Quản lý độ khó của bản đồ

        [Header("UI Elements")]
        [SerializeField] private GameObject lockObj; // GameObject khóa
        [SerializeField] private GameObject unlockObj; // GameObject mở khóa
        [SerializeField] private GameObject activeLevelIndicator; // Hiển thị level đang chọn
        [SerializeField] private GameObject objectAttack; // Object được thêm vào ObjectAttackManager
        [SerializeField] private Text zoneIndexText; // Text hiển thị số level
        [SerializeField] private Button btn; // Button level

        private int levelIndex; // Chỉ số của level hiện tại
        private const string GrassAreaName = "GrassChoosen";
        private const string LavaAreaName = "LavaChoosen";

        private void Start()
        {
            levelIndex = GetLevelIndex();
            string areaName = GetAreaName();

            SetLevelButton(areaName);
            btn.onClick.AddListener(OnClick);
        }

        #region UI Setup
        private void SetUnlockedUI()
        {
            lockObj.SetActive(false);
            unlockObj.SetActive(true);
            activeLevelIndicator.SetActive(false);
        }

        private void SetLockedUI()
        {
            lockObj.SetActive(true);
            unlockObj.SetActive(false);
            activeLevelIndicator.SetActive(true);
        }
        #endregion

        #region Helper Methods
        private int GetLevelIndex() => transform.GetSiblingIndex() - 1;

        private string GetAreaName() => transform.parent.parent.name;

        private int GetAreaIndex(string areaName) => areaName switch
        {
            GrassAreaName => 0,
            LavaAreaName => 1,
            _ => 0
        };

        private MapType GetMapType(string areaName) => areaName switch
        {
            GrassAreaName => MapType.Grass,
            LavaAreaName => MapType.Lava,
            _ => MapType.Grass
        };

        private bool IsLevelUnlocked(int areaIndex, int levelIndex)
        {
            if (levelIndex == 0 && levelIndex == 0) return true;

            if (areaIndex < 0 || areaIndex >= MapManager.Instance.MapArrayData.Length) return false;
            if (levelIndex < 0 || levelIndex >= MapManager.Instance.MapArrayData[areaIndex].MapSOArray.Length) return false;

            return MapManager.Instance.MapArrayData[areaIndex].MapSOArray[levelIndex].Unlocked;
        }
        #endregion

        #region Core Logic
        public void SetLevelButton(string areaName)
        {
            int areaIndex = GetAreaIndex(areaName);
            bool isUnlocked = IsLevelUnlocked(areaIndex, levelIndex);

            if (isUnlocked)
            {
                SetUnlockedUI();
                mapDataSO = LevelUIManager.Instance.GetMapSO(levelIndex, GetMapType(areaName));
                levelInfo = LevelUIManager.Instance.LevelInfo;
                changeDifficultMapInfos = LevelUIManager.Instance.ChangeDifficultMap;

                if (LevelUIManager.Instance.CurrentLevelButton == this)
                {
                    OnClick();
                }
            }
            else
            {
                SetLockedUI();
                zoneIndexText.text = string.Empty;
            }
        }

        public void OnClick()
        {
            if (levelInfo == null || mapDataSO == null) return;

            MapManager.Instance.Difficult = Difficult.Easy;
            levelInfo.SetLevelDataDifficulty(mapDataSO, null);
            levelInfo.OnButtonClickUIChooseMap();
            MapManager.Instance.SetCurrentMapSO(mapDataSO);

            if (objectAttack != null)
            {
                ObjectAttackManager.Instance.AddObjectToManager(objectAttack);
            }

            ActivateUnlockDifficultyMap(changeDifficultMapInfos);
        }

        private void ActivateUnlockDifficultyMap(ChangeDifficultMapInfos changeDifficultMapInfos)
        {
            if (changeDifficultMapInfos == null || !changeDifficultMapInfos.gameObject.activeSelf)
            {
                Debug.LogWarning("ChangeDifficultMapInfos is either null or inactive.");
                return;
            }

            changeDifficultMapInfos.SetUnlockDifficult(mapDataSO);
            changeDifficultMapInfos.SetHolderPVP(Difficult.Easy);
        }
        #endregion
    }
}
