using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIGameDataMap;
using UnityEngine;

public class PortalSpawnManager : AbilityPointAbstract
{
    private static PortalSpawnManager instance;
    public static PortalSpawnManager Instance => instance;

    [SerializeField] protected PortalSpawnAction portalSpawnAction;
    public PortalSpawnAction PortalSpawnAction => portalSpawnAction;
    [SerializeField] protected ProgressPortals progressPortals;
    public ProgressPortals ProgressPortals => progressPortals;
    [SerializeField] protected PortalTimer portalTimer;
    public PortalTimer PortalTimer => portalTimer;

    [Header("ListPortals Spawning")]
    public List<Portals> portalsSpawning;

    [Header("MapSO")]
    [SerializeField] MapSO mapSO;
    public MapSO MapSO { get { return mapSO; } set { mapSO = value; } }


    //Envent
    public static event Action AllPortalsSpawned;

    protected override void Awake()
    {
        base.Awake();
        //if (PortalSpawnManager.instance != null) Debug.LogError("Only 1 PortalSpawnManagerCtrl Warning");
        PortalSpawnManager.instance = this;
    }
    protected override void Start()
    {
        base.Start();
        portalsSpawning = this.mapSO.PortalsSpawn(mapSO).ToList();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadPortalSpawnAction();
        this.loadProgressPortals();
        this.loadPortalTimer();
    }
    protected virtual void loadPortalSpawnAction()
    {
        if (this.portalSpawnAction != null) return;
        this.portalSpawnAction = transform.GetComponentInChildren<PortalSpawnAction>();
        Debug.Log(gameObject.name + ": loadPortalSpawning" + gameObject);
    }
    protected virtual void loadPortalTimer()
    {
        if (this.portalTimer != null) return;
        this.portalTimer = transform.GetComponentInChildren<PortalTimer>();
        Debug.Log(gameObject.name + ": loadPortalSpawning" + gameObject);
    }
    protected virtual void loadProgressPortals()
    {
        if (this.progressPortals != null) return;
        this.progressPortals = transform.GetComponentInChildren<ProgressPortals>();
        Debug.Log(gameObject.name + ": loadProgressPortals" + gameObject);
    }
    public void CheckPortalsSpawned()
    {
        //if (portalsSpawning.Count == 0)
        //{
        //    // Trigger event when list is empty
        //    AllPortalsSpawned?.Invoke();
        //}
        if (portalsSpawning.Count == 0)
        {
            // Trigger event when list is empty
            Debug.Log("Đã hết kẻ địch , qua màn");
            AllPortalsSpawned?.Invoke();
        }

    }
}