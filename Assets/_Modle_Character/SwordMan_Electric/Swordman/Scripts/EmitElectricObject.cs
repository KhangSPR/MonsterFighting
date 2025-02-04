using System.Collections.Generic;
using UnityEngine;

public struct LineData
{
    public GameObject StartObject;
    public GameObject EndObject;
    public LineRenderer LineRenderer;
}

public class EmitElectricObject : MonoBehaviour
{   
    [Header("Manager settings")]
    public LayerMask targetLayer; // Layer for objects that can be chained
    public float chainRange = 5f; // Range for the electric chain
    public int maxChainLength = 5; // Maximum number of chains
    private int currentChainLength;
    private int currentChainLengthIndex;

    [Header("Line renderer settings")]
    [SerializeField] private GameObject pfElectricLine;
    [SerializeField] private GameObject pfElectricExplosion;
    private LineRenderer sourceLineRenderer;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    [SerializeField] private Texture[] textures;

    private int animationStep;

    [SerializeField] private float fps = 30;
    private float fpsCounter;

    private List<GameObject> chainedObjects = new List<GameObject>();
    private List<Collider2D> sortedColliders = new List<Collider2D>();

    private List<LineData> lineDatas = new List<LineData>();

    private void Awake()
    {
        sourceLineRenderer = pfElectricLine.GetComponent<LineRenderer>();
    }

    public void OnEnable()
    {
        chainedObjects.Clear();
        sortedColliders.Clear();
        lineDatas.Clear();
        currentChainLength = 0;
        currentChainLengthIndex = 0;
        fpsCounter = 0;
        
        chainedObjects.Add(gameObject);
        ChainEffect();
    }

    public void OnDisable()
    {
        for (int i = 0; i < lineDatas.Count; i++)
        { 
            lineDatas[i].LineRenderer.enabled = false;
        }
    }

    private void Update()
    {   
        fpsCounter += Time.deltaTime;

        if (fpsCounter >= 1f / fps) {
            animationStep++;
            if (animationStep == textures.Length) animationStep = 0;
            fpsCounter = 0;
        }

        for (int i = 0; i < lineDatas.Count; i++)
        {   
            lineDatas[i].LineRenderer.SetPosition(0, lineDatas[i].StartObject.transform.position);
            lineDatas[i].LineRenderer.SetPosition(1, lineDatas[i].EndObject.transform.position);

            if (textures.Length > 0) lineDatas[i].LineRenderer.material.SetTexture("_MainTex", textures[animationStep]);
        }
    }

    private void ChainEffect()
    {   
        while (currentChainLength < maxChainLength && currentChainLengthIndex < chainedObjects.Count) {
            List<List<GameObject>> uncheckedObjects = new List<List<GameObject>>();

            // Find closest objects
            for (int i = currentChainLengthIndex; i < chainedObjects.Count; i++)
            {   
                List<GameObject> tempList = new List<GameObject>{chainedObjects[i]};
                sortedColliders = GetNextObjects(chainedObjects[i]);
                for (int j = 0; j < sortedColliders.Count; j++) tempList.Add(sortedColliders[j].gameObject);

                uncheckedObjects.Add(tempList);
            }
            currentChainLengthIndex = chainedObjects.Count;

            // Check duplicate and Update line renderers to available objects
            for (int i = 0; i < uncheckedObjects.Count; i++)
            {   
                for (int j = 1; j < uncheckedObjects[i].Count; j++) {
                    if (currentChainLength >= maxChainLength) return;

                    if (!chainedObjects.Contains(uncheckedObjects[i][j]))
                    {   
                        chainedObjects.Add(uncheckedObjects[i][j]);

                        AddLineRenderer(uncheckedObjects[i][0], uncheckedObjects[i][j]);

                        // Emit electric effect here (e.g., play particle, deal damage, etc.)
                        //Debug.Log($"Chained to: {uncheckedObjects[i][j].name}");

                        currentChainLength++;
                    }
                }
            }
        }
    }

    private void AddLineRenderer(GameObject startObject, GameObject currentObject)
    {   
        LineRenderer tempLineRenderer = currentObject.GetComponent<LineRenderer>();
        if (tempLineRenderer == null) {
            tempLineRenderer = currentObject.AddComponent<LineRenderer>();
            CopyLineRendererSettings(tempLineRenderer);
        }
        else tempLineRenderer.enabled = true;

        if (!currentObject.transform.Find(pfElectricExplosion.name)) {
            GameObject tmpObj = Instantiate(pfElectricExplosion, currentObject.transform.position, Quaternion.identity);
            tmpObj.transform.SetParent(currentObject.transform);
        }
        else currentObject.transform.Find(pfElectricExplosion.name).gameObject.SetActive(true);

        LineData newLine = new LineData
        {
            StartObject = startObject,
            EndObject = currentObject,
            LineRenderer = tempLineRenderer
        };

        lineDatas.Add(newLine);
    }

    private List<Collider2D> GetNextObjects(GameObject currentObject)
    {
        // Find nearby objects
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(currentObject.transform.position, chainRange, targetLayer);

        // Sort colliders by distance to the current object
        List<Collider2D> sortedColliders = new List<Collider2D>(nearbyColliders);
        sortedColliders.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(currentObject.transform.position, a.transform.position);
            float distanceB = Vector2.Distance(currentObject.transform.position, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });

        return sortedColliders;
    }

    private void OnDrawGizmos()
    {
        // Draw a debug range for visualization
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chainRange);
    }

    private void CopyLineRendererSettings(LineRenderer destination)
    {   
        destination.positionCount = sourceLineRenderer.positionCount;
        destination.startWidth = sourceLineRenderer.startWidth;
        destination.endWidth = sourceLineRenderer.endWidth;
        destination.materials = sourceLineRenderer.sharedMaterials;
        destination.colorGradient = sourceLineRenderer.colorGradient;
        //destination.widthCurve = sourceLineRenderer.widthCurve;
        destination.numCapVertices = sourceLineRenderer.numCapVertices;
        destination.numCornerVertices = sourceLineRenderer.numCornerVertices;
        destination.loop = sourceLineRenderer.loop;
        destination.textureMode = sourceLineRenderer.textureMode;
        destination.alignment = sourceLineRenderer.alignment;

        destination.sortingOrder = sourceLineRenderer.sortingOrder;
        destination.textureScale = sourceLineRenderer.textureScale;
        

        // Copy positions
        // Vector3[] positions = new Vector3[sourceLineRenderer.positionCount];
        // sourceLineRenderer.GetPositions(positions);
        // destination.SetPositions(positions);
    }
}
