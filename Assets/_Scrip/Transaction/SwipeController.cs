using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SwipeController : MonoBehaviour,IEndDragHandler
{
    [SerializeField] int maxPage;
    public int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    [SerializeField] float dragThreshould;
    private void Awake()
    {
        currentPage = 0;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
    }
    public void Next()
    {
        if(currentPage < maxPage-1)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if(currentPage > 0)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    public void MovePage()
    {
        float newTargetPosX = ( currentPage ) * pageStep.x;
        Debug.Log(newTargetPosX);
        levelPagesRect.LeanMoveX(newTargetPosX, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {

                Debug.Log("Drag Left");
                Previous();
            }

            else
            {
                Debug.Log("Drag Right");
                Next(); 
            } 
        } else
        {
            Debug.Log("Drag Current");
            MovePage();
        }
    }
}
