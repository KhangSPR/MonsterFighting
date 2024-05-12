using UnityEngine;
using UnityEngine.UI;

public class ChooseeCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject Fade;
    [SerializeField] Button button;
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnEnable()
    {
        Fade.SetActive(true);
        //CardManager.Instance.InstanceALLCard();
    }

    private void OnDisable()
    {
        Fade.SetActive(false);
        CardManager.Instance.RemoALlCard();
    }
    private void OnButtonClick()
    {
        gameObject.SetActive(false);
    }
    //private void OnButtonClicked()
    //{
    //    if (GameManager.Instance.OnFinish)
    //    {
    //        if (uIInGame != null && uIInGame.CardTowerData != null)
    //        {
    //            this.uIInGame.CardTowerData.InstantiateObjectsFromData();
    //        }
    //        btnUI.OnClickButton();
    //    }
    //    else
    //    {
    //        if (uIInGame != null && uIInGame.CardTowerData != null)
    //        {
    //            this.uIInGame.CardTowerData.InstantiateObjectsFromData();
    //        }
    //        gameObject.SetActive(false);
    //    }
    //}
}
