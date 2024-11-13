using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleCtrl : SaiMonoBehaviour
{
    [Header("Castle Ctrl")]
    [SerializeField] protected  CastleDamageReceiver castleDamageReceiver;
    public CastleDamageReceiver CastleDamageReceiver => castleDamageReceiver;

    [SerializeField] protected Despawn despawn;
    public Despawn Despawn => despawn;
    [SerializeField] protected GameObject objBreak;
    public GameObject ObjBreak => objBreak;
    [SerializeField] protected Transform modle;
    public Transform Modle { get => modle; }
    [SerializeField] protected Transform objMove;
    public Transform ObjMove { get => objMove; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadCastleDamageReceiver();
        this.loadDespawn();
        this.loadObjBreak();
        this.loadModle();
        this.loadObjMove();
    }
    protected virtual void loadCastleDamageReceiver()
    {
        if (this.castleDamageReceiver != null) return;
        this.castleDamageReceiver = transform.GetComponentInChildren<CastleDamageReceiver>();
        Debug.Log(gameObject.name + ": loadCastleDamageReceiver" + gameObject);
    }
    protected virtual void loadDespawn()
    {
        if (this.despawn != null) return;
        this.despawn = transform.GetComponentInChildren<Despawn>();
        Debug.Log(gameObject.name + ": loadDespawn" + gameObject);
    }
    protected virtual void loadObjBreak()
    {
        if (this.objBreak != null && !this.objBreak.activeSelf) return;

        this.objBreak = transform.Find("ObjBreak")?.gameObject;
        if (this.objBreak != null)
        {
            this.objBreak.SetActive(false);
            Debug.Log(gameObject.name + ": loadObjBreak " + gameObject);
        }
    }
    protected virtual void loadModle()
    {
        if (this.modle != null) return;
        this.modle = transform.Find("Modle");
        Debug.Log(gameObject.name + ": loadModle" + gameObject);
    }
    protected virtual void loadObjMove()
    {
        if (this.objMove != null) return;
        this.objMove = transform.Find("ObjTargetMoveCity");
        Debug.Log(gameObject.name + ": loadModle" + gameObject);
    }
}
