using UIGameDataMap;
using UnityEngine;

public class UIMap : MonoBehaviour
{
    [SerializeField] Transform levelBtnGridHolderGame;
    [SerializeField] LevelInfo levelInfo;
    public LevelInfo LevelInfo => levelInfo;

    private bool initialized = false;

    private void OnEnable()
    {

        if (!initialized)
        {
            //LevelUIManager.Instance.InitializeUI(levelBtnGridHolderGame);
            //LevelUIManager.Instance.AutoClickLvInGame();
            initialized = true;
            Debug.Log("1 Lan");
        }
    }

    private void OnDisable()
    {

    }
}
