using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour
{
    [Space]
    [Header("Clan -- Name")]
    [SerializeField] Image _clanEmptyLabel;
    [SerializeField] Image _clanLabel;
    [SerializeField] TMP_Text _nameLabel;
    [Space]
    [Header("Before Info Stats")]
    [SerializeField] RectTransform holderLvInfo;
    [SerializeField] TMP_Text _levelLabel;
    [SerializeField] TMP_Text _hpLabel;
    [SerializeField] TMP_Text _attackLabel;
    [SerializeField] TMP_Text _deffLabel;
    [SerializeField] TMP_Text _speedAttackLabel;
    [SerializeField] TMP_Text _manaLabel;
    [SerializeField] TMP_Text _recoverLabel;

    [Space]
    [Header("After Info Stats")]
    [SerializeField] RectTransform holderLvUp;
    [SerializeField] TMP_Text _levelUPLabel;
    [SerializeField] TMP_Text _hpLvUPLabel;
    [SerializeField] TMP_Text _attackLvUPLabel;
    [SerializeField] TMP_Text _deffLvUPLabel;
    [SerializeField] TMP_Text _speedAttackLvUPLabel;
    [SerializeField] TMP_Text _manaLvUPLabel;
    [SerializeField] TMP_Text _recoverLvUPLabel;
    [Space]
    [Header("Skill")]
    [SerializeField] TMP_Text _skillLabel;
    [SerializeField] TMP_Text _specialLabel;
    [Space]
    [Header("Button")]
    [SerializeField] Button _btnDialogLabel;
    [SerializeField] Button _btnOKLVUpLabel;
    [SerializeField] Button _btnOnClickAvatarLabel;
    //Dialog
    [Header("Dialog")]
    [SerializeField] Notification notification;
    [SerializeField] TMP_Text _lvClick;

    //Cardcurrent
    CardPlayer cardPlayer;

    [SerializeField] float animationDuration = 0.5f; // Thời gian hiệu ứng
    private void OnEnable()
    {
        PlayerManager.OnSkillLvBefore += OnSetupBeforeInfo;
        PlayerManager.OnSkillLvAfter += OnSetupAfterInfo;
    }
    private void OnDisable()
    {
        PlayerManager.OnSkillLvBefore -= OnSetupBeforeInfo;
        PlayerManager.OnSkillLvAfter -= OnSetupAfterInfo;
    }
    private void Start()
    {
        if(_btnDialogLabel != null)
            _btnDialogLabel.onClick.AddListener(ShowNotification);
        if(_btnOKLVUpLabel != null) 
            _btnOKLVUpLabel.onClick.AddListener(ActiveFalse);
        if (_btnOnClickAvatarLabel != null)
            _btnOnClickAvatarLabel.onClick.AddListener(OnClickAvatar);
    }
    #region Dialog
    public void SetDiaLog(CardPlayer cardPlayer)
    {
        this.cardPlayer = cardPlayer;

        _levelLabel.text = "Level" + PlayerManager.Instance.LvPlayer;
        _nameLabel.text = cardPlayer.nameCard.ToString();

        _hpLabel.text = "HP: " + cardPlayer.CharacterStats.Life;
        _attackLabel.text = "Attack: " + cardPlayer.CharacterStats.Attack;
        _deffLabel.text = "Deffend: " + cardPlayer.CharacterStats.Deff;
        _speedAttackLabel.text = "SpeedAttack: " + cardPlayer.CharacterStats.AttackSpeed;
        _manaLabel.text = "Mana: " + cardPlayer.CharacterStats.Mana;
        _recoverLabel.text = "RecoverMana: " + cardPlayer.CharacterStats.RecoverMana;

        _skillLabel.text = "Skill: " + cardPlayer.skill1.skillName;
        _specialLabel.text = "Special: " + cardPlayer.skill2.skillName;
    }
    private void ShowNotification()
    {
        notification.cardPlayer = cardPlayer;

        notification.gameObject.SetActive(true);
    }
    #endregion
    public void OnClickAvatar()
    {
        this.OnDootWAnimation();
        this.UpdateIconClan();
        this.ShowGameObject(false);

        CardPlayer cardPlayer = PlayerManager.Instance.CardCurrentPlayer;

        _lvClick.text = "Level" + PlayerManager.Instance.LvPlayer;
        _nameLabel.text = cardPlayer.nameCard.ToString();

        _hpLabel.text = "HP: " + cardPlayer.CharacterStats.Life;
        _attackLabel.text = "Attack: " + cardPlayer.CharacterStats.Attack;
        _deffLabel.text = "Deffend: " + cardPlayer.CharacterStats.Deff;
        _speedAttackLabel.text = "SpeedAttack: " + cardPlayer.CharacterStats.AttackSpeed;
        _manaLabel.text = "Mana: " + cardPlayer.CharacterStats.Mana;
        _recoverLabel.text = "RecoverMana: " + cardPlayer.CharacterStats.RecoverMana;

        _skillLabel.text = "Skill: " + cardPlayer.skill1.skillName;
        _specialLabel.text = "Special: " + cardPlayer.skill2.skillName;
    }
    private void OnSetupBeforeInfo(CardPlayer cardPlayer)
    {
        this.OnDootWAnimation();
        this.UpdateIconClan();
        this.ShowGameObject(true);

        this.cardPlayer = cardPlayer;

        _levelLabel.text = "Level" + (PlayerManager.Instance.LvPlayer - 1).ToString();
        _nameLabel.text = cardPlayer.nameCard.ToString();

        _hpLabel.text = "HP: " + cardPlayer.CharacterStats.Life;
        _attackLabel.text = "Attack: " + cardPlayer.CharacterStats.Attack;
        _deffLabel.text = "Deffend: " + cardPlayer.CharacterStats.Deff;
        _speedAttackLabel.text = "SpeedAttack: " + cardPlayer.CharacterStats.AttackSpeed;
        _manaLabel.text = "Mana: " + cardPlayer.CharacterStats.Mana;
        _recoverLabel.text = "RecoverMana: " + cardPlayer.CharacterStats.RecoverMana;

        _skillLabel.text = "Skill: " + cardPlayer.skill1.skillName;
        _specialLabel.text = "Special: " + cardPlayer.skill2.skillName;
    }
    private void OnSetupAfterInfo(CardPlayer cardPlayer)
    {
        _levelUPLabel.text = "Level" + PlayerManager.Instance.LvPlayer;

        _nameLabel.text = cardPlayer.nameCard.ToString();

        _hpLvUPLabel.text = "HP: " + cardPlayer.CharacterStats.Life;
        _attackLvUPLabel.text = "Attack: " + cardPlayer.CharacterStats.Attack;
        _deffLvUPLabel.text = "Deffend: " + cardPlayer.CharacterStats.Deff;
        _speedAttackLvUPLabel.text = "SpeedAttack: " + cardPlayer.CharacterStats.AttackSpeed;
        _manaLvUPLabel.text = "Mana: " + cardPlayer.CharacterStats.Mana;
        _recoverLvUPLabel.text = "RecoverMana: " + cardPlayer.CharacterStats.RecoverMana;

        _skillLabel.text = "Skill: " + cardPlayer.skill1.skillName;
        _specialLabel.text = "Special: " + cardPlayer.skill2.skillName;
    }
    private void UpdateIconClan()
    {
        GuildSO guildSo = GuildManager.Instance.GuildJoined;

        if (guildSo != null)
        {
            _clanLabel.gameObject.SetActive(true);
            _clanLabel.sprite = guildSo.GuildIcon;
            _clanEmptyLabel.gameObject.SetActive(false);
        }
    }
    private void OnDootWAnimation()
    {
        transform.DOScale(Vector3.one, animationDuration)
        .From(Vector3.zero)
        .SetEase(Ease.OutBack);
    }
    private void ActiveFalse()
    {
        transform.DOScale(Vector3.zero, animationDuration)
        .SetEase(Ease.InBack);
    }
    private void ShowGameObject(bool active)
    {
        if(_levelUPLabel!=null)
            _levelUPLabel.gameObject.SetActive(active);
        if (_levelLabel != null)
            _levelLabel.gameObject.SetActive(active);
        if (holderLvUp != null)
            holderLvUp.gameObject.SetActive(active);
        if (_lvClick != null)
            _lvClick.gameObject.SetActive(!active);
    }
}
