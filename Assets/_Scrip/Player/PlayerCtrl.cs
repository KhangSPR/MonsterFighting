using UnityEngine;
using UIGameDataManager;

public class PlayerCtrl : ObjectCtrl
{
    [Header("Player Ctrl")]
    [SerializeField] protected CardCharacter cardCharacter;
    public CardCharacter CardCharacter => cardCharacter;
    [SerializeField] protected PlayerAttack playerAttack;
    public PlayerAttack PlayerAttack => playerAttack;
    [SerializeField] protected PlayerShooter playerShooter;
    public PlayerShooter PlayerShooter => playerShooter;
    [SerializeField] protected TargetVFX targetVFX;
    public TargetVFX TargetVFX => targetVFX;
    private Vector3Int cellPosition;
    StatsFake characterStatsFake;
    public StatsFake CharacterStatsFake => characterStatsFake;
    protected override void OnEnable()
    {
        base.OnEnable();

        this.abstractModel.SetOnDeadAnimation();
    }
    private void InitCharacterStats()
    {
        if (cardCharacter != null)
        {
            // Kiểm tra nếu characterStatsFake đã được khởi tạo
            if (characterStatsFake == null)
            {
                characterStatsFake = gameObject.AddComponent<StatsFake>();
                characterStatsFake.Initialize(cardCharacter.CharacterStats.Attack, cardCharacter.CharacterStats.Life, cardCharacter.CharacterStats.AttackSpeed, cardCharacter.CharacterStats.CurrentManaAttack);
            }
            else
            {
                characterStatsFake.Initialize(cardCharacter.CharacterStats.Attack, cardCharacter.CharacterStats.Life, cardCharacter.CharacterStats.AttackSpeed, cardCharacter.CharacterStats.CurrentManaAttack);
            }

            // Áp dụng thêm các chỉ số từ GuildSOManager (nếu cần)
            if (GameManager.Instance.GuildSOManager.GuildJoined != null)
            {
                GameManager.Instance.GuildSOManager.GuildJoined.abilitySO.ApplyMoreStats(gameObject);
            }
        }
    }
    private void SetSkill(CardCharacter cardTower)
    {
        // Skill 1
        SkillSO skill1 = cardTower.skill1;
        float manaSkill1 = skill1 != null ? skill1.manaRequirement : 0f;
        float dmg1 = skill1 != null ? skill1.damage : 0f;
        bool lockSkill1 = skill1 != null ? skill1.skillUnlock : false;
        ISkill classSkill1 = skill1 != null ? skill1.GetSkillInstance() : null;

        // Skill 2
        SkillSO skill2 = cardTower.skill2;
        float manaSkill2 = skill2 != null ? skill2.manaRequirement : 0f;
        bool lockSkill2 = skill2 != null ? skill2.skillUnlock : false;
        float dmg2 = skill2 != null ? skill2.damage : 0f;
        ISkill classSkill2 = lockSkill2 && skill2 != null ? skill2.GetSkillInstance() : null;

        // Call Function SetSkill for AbstractModel
        this.abstractModel.SetSkill(manaSkill1, lockSkill1,dmg1, classSkill1, manaSkill2, lockSkill2, dmg2, classSkill2);
    }



    public void ApplyTemporaryStats(StatsFake tempStats)
    {
        if (characterStatsFake != null)
        {
            // Áp dụng chỉ số tạm thời từ tempStats vào characterStatsFake
            characterStatsFake.Attack = tempStats.Attack;
            characterStatsFake.Life = tempStats.Life;
            characterStatsFake.AttackSpeed = tempStats.AttackSpeed;
            characterStatsFake.CurrentMana = tempStats.CurrentMana;

            // Áp dụng các chỉ số vào GameObject nếu cần thiết
        }
    }

    public void SetCardTower(CardCharacter cardTower)
    {
        cardCharacter = cardTower;
        InitCharacterStats();
        SetSkill(cardTower);
    }

    protected override string GetObjectTypeString()
    {
        return ObjectType.Player.ToString();
    }

    public virtual void Init(Vector3Int cellPos)
    {
        cellPosition = cellPos;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerAttack();
        LoadPlayerShooter();
        loadTargetVFX();
    }

    protected virtual void LoadPlayerAttack()
    {
        if (playerAttack != null) return;
        playerAttack = transform.GetComponentInChildren<PlayerAttack>();
        Debug.Log(gameObject.name + ": loadPlayerAttack" + gameObject);
    }
    protected virtual void loadTargetVFX()
    {
        if (targetVFX != null) return;
        targetVFX = transform.GetComponentInChildren<TargetVFX>();
        Debug.Log(gameObject.name + ": loadTargetVFX" + gameObject);
    }

    protected virtual void LoadPlayerShooter()
    {
        if (playerShooter != null) return;
        playerShooter = transform.GetComponentInChildren<PlayerShooter>();
        Debug.Log(gameObject.name + ": loadCanAttackPlayer" + gameObject);
    }
}
