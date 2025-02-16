﻿using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Explodable : MonoBehaviour
{
    public System.Action<List<GameObject>> OnFragmentsGenerated;

    public Material fragmentMaterial;

    public bool allowRuntimeFragmentation = false;
    public int extraPoints = 0;
    public int subshatterSteps = 0;

    public string fragmentLayer = "Default";
    public string sortingLayerName = "Default";
    public int orderInLayer = 0;

    public enum ShatterType
    {
        Triangle,
        Voronoi
    };
    public ShatterType shatterType;
    public List<GameObject> fragments = new List<GameObject>();
    private List<List<Vector2>> polygons = new List<List<Vector2>>();

    private void OnEnable()
    {
        fragmentInEditor();
    }

    public void explode()
    {
        if (fragments.Count == 0 && allowRuntimeFragmentation)
        {
            generateFragments();
        }
        else
        {
            foreach (GameObject frag in fragments)
            {
                frag.transform.parent = null;
                frag.SetActive(true);
            }
        }

        if (fragments.Count > 0)
        {
            StartCoroutine(DestroyFragmentsAfterDelay(1f)); // Destroy fragments after 1 second
            // Destroy(gameObject); // Uncomment if needed
        }
    }

    private IEnumerator DestroyFragmentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject frag in fragments)
        {
            if (frag != null)
            {
                Destroy(frag);
            }
        }
        fragments.Clear();
    }

    public void fragmentInEditor()
    {
        if (fragments.Count > 0)
        {
            deleteFragments();
        }
        generateFragments();
        setPolygonsForDrawing();
        foreach (GameObject frag in fragments)
        {
            frag.transform.parent = transform;
            frag.SetActive(false);
        }
    }

    public void deleteFragments()
    {
        foreach (GameObject frag in fragments)
        {
            if (frag != null)
            {
                DestroyImmediate(frag); // Editor-only destruction
            }
        }
        fragments.Clear();
        polygons.Clear();
    }

    private void generateFragments()
    {
        fragments = new List<GameObject>();
        switch (shatterType)
        {
            case ShatterType.Triangle:
                fragments = SpriteExploder.GenerateTriangularPieces(gameObject, extraPoints, subshatterSteps, mat: fragmentMaterial);
                break;
            case ShatterType.Voronoi:
                fragments = SpriteExploder.GenerateVoronoiPieces(gameObject, extraPoints, subshatterSteps);
                break;
            default:
                Debug.Log("invalid choice");
                break;
        }

        int layer = LayerMask.NameToLayer("UI");
        if (layer == -1)
        {
            Debug.LogError("Invalid layer name: " + fragmentLayer);
            return;
        }

        foreach (GameObject p in fragments)
        {
            if (p != null)
            {
                p.layer = layer;
                Renderer fragmentRenderer = p.GetComponent<Renderer>();
                if (fragmentRenderer != null)
                {
                    fragmentRenderer.sortingLayerName = sortingLayerName;
                    fragmentRenderer.sortingOrder = orderInLayer;
                }
            }
        }

        foreach (ExplodableAddon addon in GetComponents<ExplodableAddon>())
        {
            if (addon.enabled)
            {
                addon.OnFragmentsGenerated(fragments);
            }
        }
    }

    private void setPolygonsForDrawing()
    {
        polygons.Clear();
        List<Vector2> polygon;

        foreach (GameObject frag in fragments)
        {
            polygon = new List<Vector2>();
            foreach (Vector2 point in frag.GetComponent<PolygonCollider2D>().points)
            {
                Vector2 offset = rotateAroundPivot((Vector2)frag.transform.position, (Vector2)transform.position, Quaternion.Inverse(transform.rotation)) - (Vector2)transform.position;
                offset.x /= transform.localScale.x;
                offset.y /= transform.localScale.y;
                polygon.Add(point + offset);
            }
            polygons.Add(polygon);
        }
    }

    private Vector2 rotateAroundPivot(Vector2 point, Vector2 pivot, Quaternion angle)
    {
        Vector2 dir = point - pivot;
        dir = angle * dir;
        point = dir + pivot;
        return point;
    }

    void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            if (polygons.Count == 0 && fragments.Count != 0)
            {
                setPolygonsForDrawing();
            }

            Gizmos.color = Color.blue;
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector2 offset = (Vector2)transform.position * 0;
            foreach (List<Vector2> polygon in polygons)
            {
                for (int i = 0; i < polygon.Count; i++)
                {
                    if (i + 1 == polygon.Count)
                    {
                        Gizmos.DrawLine(polygon[i] + offset, polygon[0] + offset);
                    }
                    else
                    {
                        Gizmos.DrawLine(polygon[i] + offset, polygon[i + 1] + offset);
                    }
                }
            }
        }
    }
}
