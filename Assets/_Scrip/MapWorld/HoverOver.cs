using UIGameDataManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : SaiMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] FullMapController fullMapCtrl;
    [SerializeField] Animator animator;
    [SerializeField] GameObject GlowEffect;
    [SerializeField] int indexMap;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadfullMapCtrl();

    }
    protected void LoadfullMapCtrl()
    {
        if (fullMapCtrl != null) return;
        fullMapCtrl = transform.parent.parent.GetComponent<FullMapController>();
        animator = transform.parent.GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(transform.name + "Enter");
        //if (!IsMapOpening())
        //{
        //    PlayAnimationByName(transform.name);

        //    Debug.Log("OnPointerEnter");
        //}
    }
    public void PlayAnimationByName(string nameAnimation)
    {
        if (animator == null) return;

        // Đặt animation về trạng thái ban đầu trước khi phát lại
        animator.Play(nameAnimation, -1, 0f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //GlowEffect.SetActive(false);
        //Debug.Log("OnPointerExit");

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        PlayAnimationByName(transform.name);
        fullMapCtrl.OpenMap(indexMap);
        //Debug.Log(index);
        BtnUI btn = GetComponent<BtnUI>();
        btn.OnClickButton();

        Debug.Log("OnPointerDown");

        GameDataManager.Instance.ResourceMapUpdated(); //Update Resource


    }
    bool IsMapOpening()
    {
        return fullMapCtrl.isMapOpening;
    }
}
