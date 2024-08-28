using UnityEngine;

public class UIRotateImage : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        // Xoay liên tục theo trục Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
