using UnityEngine;

public class ObjectAttackManager : MonoBehaviour
{
    [SerializeField] GameObject currentAttack;


    private static ObjectAttackManager instance;
    public static ObjectAttackManager Instance => instance;


    private void Awake()
    {
        if (ObjectAttackManager.instance != null)
        {
            Debug.LogError("Only 1 ObjectAttackManager Warning");
        }
        ObjectAttackManager.instance = this;
    }
    public void AddObjectToManager(GameObject objectAttack)
    {
        // If currentAttack already exists and is the same as objectAttack, nothing needs to be done
        if (currentAttack != null && currentAttack == objectAttack)
        {
            return;
        }

        // If currentAttack already exists, disable it
        if (currentAttack != null)
        {
            currentAttack.SetActive(false);
        }

        // Set currentAttack new
        currentAttack = objectAttack;

        // Enable new currentAttack
        currentAttack.SetActive(true);
    }

}