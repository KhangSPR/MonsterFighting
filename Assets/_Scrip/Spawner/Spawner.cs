﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : SaiMonoBehaviour
{
    [Header("Spawner")]
    public Transform holder;

    [SerializeField] protected int spawnedCount = 0;
    public int SpawnedCount => spawnedCount;

    public List<Transform> prefabs;
    [SerializeField] protected List<Transform> poolObjs;
    protected override void LoadComponents()
    {
        this.LoadPrefabs();
        this.LoadHolder();
    }

    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");
        Debug.LogWarning(transform.name + ": LoadHodler", gameObject);
    }

    protected virtual void LoadPrefabs()
    {
        if (this.prefabs.Count > 0) return;

        Transform prefabObj = transform.Find("Prefabs");
        foreach (Transform prefab in prefabObj)
        {
            this.prefabs.Add(prefab);
        }

        this.HidePrefabs();

        Debug.LogWarning(transform.name + ": LoadPrefabs", gameObject);
    }

    protected virtual void HidePrefabs()
    {
        foreach (Transform prefab in this.prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }

    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }

        return this.Spawn(prefab, spawnPos, rotation);
    }

    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPos, rotation);

        newPrefab.SetParent(this.holder);
        this.spawnedCount++;

        return newPrefab;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in this.poolObjs)
        {
            if (poolObj == null) continue;

            if (poolObj.name == prefab.name)
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }

    public virtual void Despawn(Transform obj)
    {
        if (this.poolObjs.Contains(obj))
        {
            Debug.Log("Despawn: " + obj.name);

            return;
        }

        this.poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
        this.spawnedCount--;

        Debug.Log("Despawn Cotain ! :" + obj.name);
    }

    public Transform SpawnObject(Vector3 spawnPos, Quaternion rotation)
    {
        // Gọi hàm RandomPrefab để chọn một prefab enemy ngẫu nhiên
        Transform enemyPrefab = RandomPrefab();

        // Gọi hàm Spawn để tạo đối tượng enemy từ prefab đã chọn
        Transform enemy = Spawn(enemyPrefab, spawnPos, rotation);

        return enemy;
    }
    public virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName) return prefab;
        }

        return null;
    }
    public virtual GameObject GetGameobjectPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName) return prefab.gameObject;
        }

        return null;
    }
    public virtual List<Transform> FindObjectEnable()
    {
        List<Transform> list = new List<Transform>();   

        foreach(Transform obj in holder)
        {
            if(obj.gameObject.activeSelf)
            {
                list.Add(obj);
            }
        }
        return list;
    }
    public virtual Transform RandomPrefab()
    {
        int rand = Random.Range(0, this.prefabs.Count);
        return this.prefabs[rand];
    }

    public virtual void Hold(Transform obj)
    {
        obj.parent = this.holder;
    }
}
