using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] Button maleSelect;
    [SerializeField] Button femaleSelect;
    [SerializeField] Button activeSelect;
    [SerializeField] Image maleImage;
    [SerializeField] Image femaleImage;
    [Space]
    [Header("Holder Skill")]
    [SerializeField] Transform holderMale;
    [SerializeField] Transform holderFeMale;
    [Space]
    [Header("Player Cards")]
    [SerializeField] CardPlayer male;
    [SerializeField] CardPlayer female;
    [SerializeField] SkillInfo skillInfo;

    private void OnEnable(){
        maleSelect.onClick.AddListener(() => {
            Debug.Log("Click");
            OnClick(true);
        });
        femaleSelect.onClick.AddListener(() => {
            Debug.Log("Click");

            OnClick(false);
        });
    }

    private void OnDisable(){
        maleSelect.onClick.RemoveAllListeners();
        femaleSelect.onClick.RemoveAllListeners();
    }
    private void OnClick(bool IsActive)
    {
        if(IsActive)
        {
            skillInfo.transform.position = holderMale.position;
            maleSelect.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            femaleSelect.GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 200 / 255f);
            skillInfo.SetDiaLog(male);


        }
        else
        {
            maleSelect.GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 200 / 255f);
            femaleSelect.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            skillInfo.SetDiaLog(female);


            skillInfo.transform.position = holderFeMale.position;

        }
        skillInfo.gameObject.SetActive(true);
    }
}
