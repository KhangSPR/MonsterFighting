using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetStar : SaiMonoBehaviour
{
    public List<Image> renderersArray;
    public bool activeArray;
    protected override void Start()
    {
        base.Start();
        activeArray = true;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRenderersArray();
    }
    protected void LoadRenderersArray()
    {
        if (renderersArray.Count > 0) return;

        Debug.Log("LoadRenderersArray");

        renderersArray.AddRange(transform.GetComponentsInChildren<Image>());
    }
    public void SetActiveImage(SpriteRenderer renderer)
    {
        foreach (var _renderer in renderersArray)
        {
            _renderer.sprite = renderer.sprite;
        }
    }
}
