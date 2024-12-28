using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour {
    [SerializeField] Button maleSelect;
    [SerializeField] Button femaleSelect;

    private void OnEnable(){
        maleSelect.onClick.AddListener(() => {
            // actorDB.AddActor("protagonist",maleActor);
            Debug.Log("ChooseCharacter Man Click");

            gameObject.SetActive(false);
            DialogManager.Instance.ChooseNext();
        });
        femaleSelect.onClick.AddListener(() => {
            // actorDB.AddActor("protagonist",femaleActor);
            gameObject.SetActive(false);

            Debug.Log("ChooseCharacter Women Click");
            DialogManager.Instance.ChooseNext();
        });
    }

    private void OnDisable(){
        maleSelect.onClick.RemoveAllListeners();
        femaleSelect.onClick.RemoveAllListeners();
    }

    public void Display(){
        Instantiate(this, FindObjectOfType<Canvas>().transform);
    }
}
