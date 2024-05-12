using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    public GameObject[] Cards;

    private void Awake()
    {
        if(instance == null )
        {
            instance = this;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(SetSelectedAfterOneFrame());
    }
    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(Cards[0]);
    }
}
