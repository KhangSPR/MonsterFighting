using UnityEngine;

public class Hover : SaiMonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;
    private static Hover _instance;
    public static Hover Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        //if (Hover._instance != null) Debug.LogError("Only 1 Hover Warning");
        Hover._instance = this;
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void FollowMouse(bool active)
    {
        if (spriteRenderer == null) return;

        if (!active)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;

        }

        //Debug.Log("FollowMouse " + active);
        if (spriteRenderer.enabled)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        }
    }
    public void Activate(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;

    }
    public void Hide()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
    public void Deactivate()
    {
        spriteRenderer.sprite = null;  // Set sprite to null when deactivating
        spriteRenderer.enabled = false;

    }
}
