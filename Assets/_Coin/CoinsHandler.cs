using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CoinsHandler : MonoBehaviour
{   
    [Header("Coin dropping settings")]
    public float minSpeed = 5f;
    public float maxSpeed = 10f;
    public float minTime = 0.5f;
    public float maxTime = 1.25f;
    public float minAngle = 30f;  // In degrees
    public float maxAngle = 150f;
    public float gravity = 9.8f;
    public float radiusLimit = 5f; // Maximum distance the coin can move

    private Vector2 startPosition;
    private float speed;
    private float angle; // In radians
    private float timeElapsed;
    private float dropTime;

    private Coroutine coroutine;

    //Bool True Falseg
    [SerializeField]
    bool hasBeenPickedUp = false;

    private void OnEnable()
    {
        startPosition = transform.position;
        
        // Randomize speed and angle
        speed = Random.Range(minSpeed, maxSpeed);
        angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        dropTime = Random.Range(minTime, maxTime);

        coroutine = StartCoroutine(UpdateEffect());


        //if (!hasBeenPickedUp) // Chỉ gọi Invoke nếu vật phẩm chưa được nhặt
        //{
        //    Invoke("ItemPickupAnimation", Random.Range(2f, 3f));
        //}
    }

    private void OnDisable()
    {
        transform.position = startPosition;
        timeElapsed = 0;
        if (coroutine != null) StopCoroutine(coroutine);

    }
    protected void OnMouseDown()
    {
        ItemPickupAnimation();
    }
    public void ItemPickupAnimation()
    {
        if (hasBeenPickedUp) return; // Kiểm tra nếu đã nhặt thì không thực hiện lại

        hasBeenPickedUp = true;

        // Lấy đối tượng mục tiêu
        UITopLeft uiTopLeft = Map_Ui_Manager.instance.UI_Top_left.GetComponent<UITopLeft>();
        Transform target = uiTopLeft.TransformsResources[0];

        // Gửi coin đến CostManager
        transform.DOMove(target.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            CostManager.Instance.Currency += 1;
            FXSpawner.Instance.Despawn(transform);
            hasBeenPickedUp = false;
            transform.DOKill();

            Debug.Log("PickupItem Complete");

        });
    }

    private void StopMovement()
    {
        // Stop updating position
        //enabled = false;

        // Optional: Snap to ground
        Vector2 currentPosition = transform.position;
        currentPosition.y = startPosition.y; // Assume ground level is startPosition.y
        transform.position = currentPosition;
    }

    private IEnumerator UpdateEffect()
    {   
        while (timeElapsed <= dropTime) {
            timeElapsed += Time.deltaTime;

            // Calculate new position using the parametric equations
            float x = speed * Mathf.Cos(angle) * timeElapsed;
            float y = speed * Mathf.Sin(angle) * timeElapsed - 0.5f * gravity * Mathf.Pow(timeElapsed, 2);

            // Update the position
            transform.position = startPosition + new Vector2(x, y);

            // Check if the coin has landed or reached its radius limit
            if (Vector2.Distance(startPosition, transform.position) >= radiusLimit)
            {
                //StopMovement();
                

                break;
            }

            yield return null;
        }
    }
}
