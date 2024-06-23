using System;
using System.Collections;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : ObjectCtrl
{
    [Header("Player Ctrl")]
    [SerializeField] int manaMax;
    [SerializeField] public int mana;
    [SerializeField] protected CardCharacter cardTower;
    public CardCharacter CardTower { get => cardTower; }
    [SerializeField] protected PlayerAttack playerAttack;
    public PlayerAttack PlayerAttack => playerAttack;
    public PlayerShooter PlayerShooter => playerShooter;
    [SerializeField] protected PlayerShooter playerShooter;
    private Vector3Int cellPosition;
    [Range(0, 3)] public int SkillLevel;
    public int skill1ManaNeed;
    public int skill2ManaNeed;
    public int skill3ManaNeed;
    [Header("UI Health Bar")]
    public Transform Canvas;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadPlayerAttack();
        this.LoadPlayerShooter();
        this.LoadPlayerUI();
    }
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaBar;
    public void LoadPlayerUI()
    {
        
        healthBar = Canvas.GetChild(0).GetComponent<Slider>();
        manaBar = Canvas.GetChild(1).GetComponent<Slider>();
        healthBar.minValue = 0;
        healthBar.maxValue = this.Receiver.IsMaxHP;
        UpdateHealhbar(this.Receiver.isHP);
        UpdateManabar(mana);
        EnableCanvas(true);
    }

    public void UpdateManabar(int mana)
    {

        Debug.Log("healthBar.value : " + healthBar.value);
        manaBar.minValue = 0;
        manaBar.maxValue = manaMax;
        manaBar.value = mana;
    }

    public void EnableCanvas(bool enable)
    {
        Canvas.gameObject.SetActive(true);

    }
    public void UpdateHealhbar(float health)
    {
        Debug.Log("Update Health Bar", healthBar.transform);
        healthBar.value = health;
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
