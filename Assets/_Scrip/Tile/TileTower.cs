using UnityEngine;
using UnityEngine.EventSystems;

public class TileTower : TileScrip
{
    public LayerMask interactableLayerMask;
    [SerializeField] private int landIndex;
    public int LandIndex => landIndex;

    private void IsActive()
    {
        if (newObjSet == null) return;

        if (!newObjSet.activeSelf)
        {
            ActionPlace();
        }
    }

    private void DeActive()
    {
        // Logic khi DeActive nếu cần
    }

    private void ActionPlace()
    {
        Place(transform, landIndex); // Truyền landIndex vào Place
        GameManager.Instance.BuyCard();
        GameManager.Instance.CardRefresh.StartCooldown();
    }

    protected override void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickBtn != null && GameManager.Instance.ClickBtn is CardButton)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    SpriteRenderer hitRenderer = hit.collider.GetComponent<SpriteRenderer>();
                    SpriteRenderer thisRenderer = GetComponent<SpriteRenderer>();

                    if (hitRenderer != null && thisRenderer != null)
                    {
                        if (hitRenderer.sortingLayerID == thisRenderer.sortingLayerID)
                        {
                            if (!IsEmpty && Input.GetMouseButtonDown(0) && hit.collider.gameObject == this.gameObject)
                            {
                                IsActive();
                                Debug.Log("On mouse Over");
                            }
                            else if (Input.GetMouseButtonDown(0) && IsEmpty && hit.collider.gameObject == this.gameObject)
                            {
                                ActionPlace();
                                Debug.Log("On mouse Over");
                            }
                        }
                    }
                }
            }
        }
    }

    protected override void OnMouseExit()
    {
        DeActive();
    }
}
