using System.Collections;
using UnityEngine;

public class ObjRageSkill : AbstractCtrl
{
    [SerializeField] private float rageDamageMultiplier;
    public float RageDamageMultiplier => rageDamageMultiplier;
    [SerializeField] private float rageSpeedMultiplier;
    public float RageSpeedMultiplier => rageSpeedMultiplier;
    [SerializeField] private float rageDuration;
    public float RageDuration => rageDuration;
    [SerializeField] private float rageHP;
    public float RageHP => rageHP;
    [SerializeField]

    private bool isRageActive = false;
    public bool IsRageActive { get { return isRageActive; } set { value = isRageActive; } }

    private IRageStrategy rageStrategy;

    [SerializeField] GameObject _furyFlash;
    public GameObject FuryFlash => _furyFlash;
    protected override void OnEnable()
    {
        base.OnEnable();

        // Đăng ký sự kiện
        this.ObjectCtrl.ObjectDamageReceiver.OnTakeDamage += OnTakeDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Hủy đăng ký sự kiện
        this.ObjectCtrl.ObjectDamageReceiver.OnTakeDamage -= OnTakeDamage;
    }

    protected override void Start()
    {
        base.Start();
        rageStrategy = new HPBasedRageStrategy();
    }

    private void OnTakeDamage()
    {
        if (!isRageActive && this.ObjectCtrl.ObjectDamageReceiver.IsHP <= this.ObjectCtrl.ObjectDamageReceiver.IsMaxHP * rageHP)
        {

            rageStrategy.ActivateRage(this.ObjectCtrl, this);

            //this.objCtrl.AbstractModel.IsRage = true;
        }
    }

    public void Activate() //Call In Strategy
    {
        if (isRageActive) return;
        isRageActive = true;
        // Kiểm tra nếu đang trong trạng thái Stun
        if (this.objCtrl.AbstractModel.IsStun)
        {
            // Chờ cho đến khi trạng thái Stun kết thúc
            StartCoroutine(WaitForStunToEndAndActivateFury());
            return;
        }

        // Kích hoạt Fury ngay nếu không bị Stun
        ActivateFury();
    }

    private IEnumerator WaitForStunToEndAndActivateFury()
    {
        // Chờ đến khi isStun trở thành false
        while (this.objCtrl.AbstractModel.IsStun)
        {
            yield return null; // Chờ 1 frame rồi kiểm tra lại
        }

        // Khi Stun kết thúc, kích hoạt Fury
        ActivateFury();
    }

    private void ActivateFury()
    {
        Debug.Log("ActivateFury");

        // Fury Gain
        this.objCtrl.AbstractModel.IsFuryGain = true;

        // VFX
        _furyFlash.SetActive(true);

        // Chuyển sang trạng thái Rage
        this.objCtrl.AbstractModel.SetRageState();

        // Bắt đầu đếm thời gian Rage
        StartCoroutine(RageDurationTimer());
    }


    private IEnumerator RageDurationTimer()
    {
        //if (this.objCtrl.ObjectDamageReceiver.IsDead)
        //{
        //    this.objCtrl.AbstractModel.IsRage = false;
        //    yield break;
        //}

        yield return new WaitForSeconds(rageDuration);

        // Phục hồi giá trị ban đầu
        ObjectCtrl.DamageSender.Damage = this.enemyCtrl.EnemySO.basePointsAttack;

        if (ObjectCtrl is EnemyCtrl enemy)
        {
            enemy.ObjMovement.MoveSpeed = this.enemyCtrl.EnemySO.basePointsSpeedMove;
        }

        this.objCtrl.AbstractModel.IsRage = false;
        //VFX
        _furyFlash.SetActive(false);


        Debug.Log("Finall Rage Timer");
    }
    public void SetRage(bool active)
    {
        _furyFlash.SetActive(active);
        isRageActive = active;
    }
}
