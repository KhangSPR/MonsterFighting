using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreViewSpawner : SaiMonoBehaviour
{

    public List<Transform> ListPreviews;


    private static PreViewSpawner instance;
    public static PreViewSpawner Instance => instance;

    protected override void Awake()
    {
        if (PreViewSpawner.instance != null)
        {
            Debug.LogError("Only 1 PreViewSpawner Warning");
        }
        PreViewSpawner.instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChildObjects();
    }

    void LoadChildObjects()
    {
        if (ListPreviews.Count < 0) return;
        // Get the transform component of the current GameObject
        Transform parentTransform = transform;

        // Iterate through all child objects
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            // Access the i-th child using GetChild(i)
            Transform child = parentTransform.GetChild(i);

            // Add the child to the ListPreviews
            ListPreviews.Add(child);

            // Do something with the child if needed
            Debug.Log("Child Object Name: " + child.name);
        }
    }
}
