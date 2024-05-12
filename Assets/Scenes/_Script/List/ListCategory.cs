using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCategory : SaiMonoBehaviour
{
    public List<GameObject> instantiatedObjects = new List<GameObject>();
    public List<string> instantiatedObjectNames = new List<string>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAllChildren(transform);
    }
    void LoadAllChildren(Transform parentTransform)
    {
        if (instantiatedObjects.Count > 0) return;
        foreach (Transform childTransform in parentTransform)
        {
            // Lấy đối tượng con
            GameObject childObject = childTransform.gameObject;

            string objectName = childObject.name;
            instantiatedObjectNames.Add(objectName);

            // Thêm đối tượng con vào danh sách
            instantiatedObjects.Add(childObject);
        }
    }
}
