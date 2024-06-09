using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionHoverOver : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Transform MissionPanel;
    public void OnPointerExit(PointerEventData eventData)
    {
        MissionPanel.gameObject.SetActive(false);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        MissionPanel.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
