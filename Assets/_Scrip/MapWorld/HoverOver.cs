using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]FullMapController fmc;
    [SerializeField]Image image;
     public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(transform.name + "Enter");
        if (!IsMapOpening())
        {
            fmc.ShowArea(image);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(transform.name + "Exit");
        fmc.HideArea(image);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {

        //Debug.Log(transform.name + "Clicked");
        
        int index = fmc.GetIndex(transform);
        fmc.OpenMap(index);
        //Debug.Log(index);
        BtnUI btn = GetComponent<BtnUI>();
        btn.OnClickButton();
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        fmc = transform.parent.parent.GetComponent<FullMapController>();
        image = GetComponent<Image>();
    }

    bool IsMapOpening()
    {
        return fmc.isMapOpening;
    }

    void SetMapOpening(bool isOpening)
    {
        fmc.isMapOpening = isOpening;
    }
}
