using System.Collections.Generic;
using UnityEngine;


//Anchors Line Renderer positions to corresponding GameObjects in the Unity Editor
[ExecuteInEditMode]
public class LineRendererAnchor : MonoBehaviour
{
    public List<GameObject> attachedObjects;
    public LineRenderer lineRenderer;
 
#if UNITY_EDITOR
    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            for (int i = 0; i < attachedObjects.Count; i++)
            {
                if (i < lineRenderer.positionCount && attachedObjects != null)
                { lineRenderer.SetPosition(i, attachedObjects[i].transform.localPosition); }
            }
        }
    }
#endif
}