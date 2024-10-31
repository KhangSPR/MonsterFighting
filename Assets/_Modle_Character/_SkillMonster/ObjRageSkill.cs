using System.Collections;
using UnityEngine;

public class ObjRageSkill : AbstractCtrl
{
    public enum RageActivationType
    {
        None,
        DurationBased,
        HitBased,
    }

    public enum RageType
    {
        NormalRage,
        AttackAxe,
    }

    [SerializeField] private RageActivationType rageActivationType = RageActivationType.DurationBased;
    public RageActivationType RageActType => rageActivationType;

    [SerializeField] private RageType currentRageType = RageType.NormalRage;
    public RageType CurrentRageType => currentRageType;

    [SerializeField] private float rageDamageMultiplier;
    public float RageDamageMultiplier => rageDamageMultiplier;
    [SerializeField] private float rageSpeedMultiplier;
    public float RageSpeedMultiplier => rageSpeedMultiplier;
    [SerializeField] private float rageDuration;
    public float RageDuration => rageDuration;
    [SerializeField] private float rageHP;
    public float RageHP => rageHP;

    private bool isRageActive = false;
    public bool IsRageActive { get { return isRageActive; } set { value = isRageActive; } }

    private IRageStrategy rageStrategy;

    [SerializeField] GameObject _furyFlash;
    public GameObject FuryFlash => _furyFlash;
    [SerializeField]
    private int hitCounter = 0; // Bộ đếm hit

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObjectCtrl.ObjectDamageReceiver.OnTakeDamage += OnTakeDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.ObjectCtrl.ObjectDamageReceiver.OnTakeDamage -= OnTakeDamage;
    }

    protected override void Start()
    {
        base.Start();
        SetRageStrategy();
    }
    private void SetRageStrategy()
    {
        rageStrategy = rageActivationType switch
        {
            RageActivationType.DurationBased => new HPBasedRageStrategy(),
            RageActivationType.HitBased => new HitBasedRageStrategy(),
            _ => null
        };
    }
    private void OnTakeDamage()
    {
        if (rageActivationType == RageActivationType.HitBased)
        {
            hitCounter++;

            Debug.Log("OnTake Damage: " + hitCounter);

            if (hitCounter >= 5)
            {
                rageStrategy.ActivateRage(this.ObjectCtrl, this);
                hitCounter = 0; // Reset lại bộ đếm sau khi kích hoạt Rage
                isRageActive = false; //Not For Single Use
            }
        }
        else if (!isRageActive && this.ObjectCtrl.ObjectDamageReceiver.IsHP <= this.ObjectCtrl.ObjectDamageReceiver.IsMaxHP * rageHP)
        {
            rageStrategy.ActivateRage(this.ObjectCtrl, this);
        }
    }
    public void Activate()
    {
        if (isRageActive) return;
        isRageActive = true;

        if (this.objCtrl.AbstractModel.IsStun)
        {
            StartCoroutine(WaitForStunToEndAndActivateFury());
            return;
        }

        ActivateFury();
    }

    private IEnumerator WaitForStunToEndAndActivateFury()
    {
        while (this.objCtrl.AbstractModel.IsStun)
        {
            yield return null;
        }

        ActivateFury();
    }

    private void ActivateFury()
    {
        this.objCtrl.AbstractModel.SetRageState(currentRageType); //Repair Not useful

        this.objCtrl.AbstractModel.IsFuryGain = true;


        if (rageActivationType != RageActivationType.DurationBased) return;

        Debug.Log("ActivateFury");


        if (_furyFlash != null)
            _furyFlash.SetActive(true);

        StartCoroutine(RageDurationTimer());
    }

    private IEnumerator RageDurationTimer()
    {
        yield return new WaitForSeconds(rageDuration);

        ObjectCtrl.DamageSender.Damage = this.enemyCtrl.EnemySO.basePointsAttack;

        if (ObjectCtrl is EnemyCtrl enemy)
        {
            enemy.ObjMovement.MoveSpeed = this.enemyCtrl.EnemySO.basePointsSpeedMove;
        }

        this.objCtrl.AbstractModel.IsRage = false;

        if (_furyFlash != null)
            _furyFlash.SetActive(false);

        Debug.Log("Finall Rage Timer");
    }

    public void SetRage(bool active)
    {
        if (_furyFlash != null)
            _furyFlash.SetActive(active);
        isRageActive = active;
    }
}
