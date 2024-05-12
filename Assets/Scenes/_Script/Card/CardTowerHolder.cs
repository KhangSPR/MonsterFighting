using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardTowerHolder : SaiMonoBehaviour
{
    // Reference to the CardTower scriptable object
    [SerializeField] private CardCharacter cardTower;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCardSO();

    }
    protected virtual void LoadCardSO() // ScriptableObject
    {
        if (cardTower != null) return;

        string resPath = "Card/CardSAOJ" + "/" + transform.parent.parent.name;
        cardTower = Resources.Load<CardCharacter>(resPath);

        Debug.LogWarning(transform.name + ": LoadCardSO" + resPath, gameObject);
    }

    // Example method to set the CardTower
    public void SetCardTower(CardCharacter newCardTower)
    {
        cardTower = newCardTower;

        // You can update the object based on the cardTower values here
        UpdateObject();
    }

    // Example method to get the CardTower
    public CardCharacter GetCardTower()
    {
        return cardTower;
    }
    // Example method to update the object based on the CardTower values
    private void UpdateObject()
    {
        // Implement logic here to update the object based on the cardTower values
        // For example, you can update UI elements, sprites, etc.
        // Example: GetComponent<SpriteRenderer>().sprite = cardTower.icon;
        // Find the child object named "Dame"
        Transform dameTransform = transform.Find("Dame");
        dameTransform.GetComponent<Text>().text = "Dame: " + cardTower.basePointsAttack.ToString();

        Transform attackSpeedTransform = transform.Find("AttackSpeed");
        attackSpeedTransform.GetComponent<Text>().text = "AttackSpeed: " + cardTower.basePointsAttackSpeed.ToString();

        Transform SkillTransform = transform.Find("Skill");
        //SkillTransform.GetComponent<Text>().text = "Skill: " + cardTower.skill.ToString();

        Transform LvTransform = transform.Find("Lv");
        //LvTransform.GetComponent<Text>().text = "Lv: " + cardTower.level.ToString();

    }

    // Example method to be called when the object is enabled
    protected override void OnEnable()
    {
        // Check if cardTower is not null before updating the object
        if (cardTower != null)
        {
            UpdateObject();
        }
    }
}
