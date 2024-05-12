using System.Collections;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : ObjectCtrl
{
    [Header("Player Ctrl")]
    [SerializeField] protected CardCharacter cardTower;
    public CardCharacter CardTower { get => cardTower; }
    [SerializeField] protected PlayerAttack playerAttack;
    public PlayerAttack PlayerAttack => playerAttack;
    public PlayerShooter PlayerShooter => playerShooter;
    [SerializeField] protected PlayerShooter playerShooter;
    private Vector3Int cellPosition;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadPlayerAttack();
        this.LoadPlayerShooter();
    }
    public void SetCardTower(CardCharacter CardTower)
    {
        cardTower = CardTower;
    }
    protected override string GetObjectTypeString()
    {
        return ObjectType.Player.ToString();
    }
    public virtual void Init(Vector3Int cellPos)
    {
        cellPosition = cellPos;
    }
    protected virtual void loadPlayerAttack()
    {
        if (this.playerAttack != null) return;
        this.playerAttack = transform.GetComponentInChildren<PlayerAttack>();
        Debug.Log(gameObject.name + ": loadPlayerAttack" + gameObject);
    }
    protected virtual void LoadPlayerShooter()
    {
        if (this.playerShooter != null) return;
        this.playerShooter = transform.GetComponentInChildren<PlayerShooter>();
        Debug.Log(gameObject.name + ": loadCanAttackPlayer" + gameObject);
    }
}
