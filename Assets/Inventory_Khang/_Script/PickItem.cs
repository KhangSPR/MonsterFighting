using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject itemObject;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        GetComponent<SpriteRenderer>().sprite = itemObject.Sprite;
        EditorUtility.SetDirty(GetComponent<SpriteRenderer>());
    }
}
