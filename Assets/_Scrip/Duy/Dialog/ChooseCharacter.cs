using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour {
    [SerializeField] Button maleSelect;
    [SerializeField] Button femaleSelect;

    [SerializeField] DialogActor maleActor;
    [SerializeField] DialogActor femaleActor;

    [SerializeField] DialogActorDatabase actorDB;

    private void OnEnable(){
        maleSelect.onClick.AddListener(() => {
            actorDB.AddActor("protagonist",maleActor);
            Destroy(gameObject);
        });
        femaleSelect.onClick.AddListener(() => {
            actorDB.AddActor("protagonist",femaleActor);
            Destroy(gameObject);
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
