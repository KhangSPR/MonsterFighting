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
    private Vector3Int cellPosition;
    StatsFake characterStatsFake;
    public StatsFake CharacterStatsFake => characterStatsFake;
    private void InitCharacterStats()
    {
        if (cardCharacter != null)
        {
            // Kiểm tra nếu characterStatsFake đã được khởi tạo
            if (characterStatsFake == null)
            {
                characterStatsFake = gameObject.AddComponent<StatsFake>();
                characterStatsFake.Initialize(cardCharacter.CharacterStats.Attack, cardCharacter.CharacterStats.Life, cardCharacter.CharacterStats.AttackSpeed, cardCharacter.CharacterStats.SpecialAttack);
            }
            else
            {
                characterStatsFake.Initialize(cardCharacter.CharacterStats.Attack, cardCharacter.CharacterStats.Life, cardCharacter.CharacterStats.AttackSpeed, cardCharacter.CharacterStats.SpecialAttack);
            }

            // Áp dụng thêm các chỉ số từ GuildSOManager (nếu cần)
            if (GameManager.Instance.GuildSOManager.GuildJoined != null)
            {
                GameManager.Instance.GuildSOManager.GuildJoined.abilitySO.ApplyMoreStats(gameObject);
            }
        }
    }

    public void ApplyTemporaryStats(StatsFake tempStats)
    {
        if (characterStatsFake != null)
        {
            // Áp dụng chỉ số tạm thời từ tempStats vào characterStatsFake
            characterStatsFake.Attack = tempStats.Attack;
            characterStatsFake.Life = tempStats.Life;
            characterStatsFake.AttackSpeed = tempStats.AttackSpeed;
            characterStatsFake.SpecialAttack = tempStats.SpecialAttack;

            // Áp dụng các chỉ số vào GameObject nếu cần thiết
        }
    }

    public void SetCardTower(CardCharacter cardTower)
    {
        cardCharacter = cardTower;
        InitCharacterStats();
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
    }

    protected virtual void LoadPlayerAttack()
    {
        if (playerAttack != null) return;
        playerAttack = transform.GetComponentInChildren<PlayerAttack>();
        Debug.Log(gameObject.name + ": loadPlayerAttack" + gameObject);
    }

    protected virtual void LoadPlayerShooter()
    {
        if (playerShooter != null) return;
        playerShooter = transform.GetComponentInChildren<PlayerShooter>();
        Debug.Log(gameObject.name + ": loadCanAttackPlayer" + gameObject);
    }
}
