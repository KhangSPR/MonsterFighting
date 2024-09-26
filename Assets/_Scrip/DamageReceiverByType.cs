// Derived class handling specific damage types
using System;
using System.Collections;
using UIGameDataMap;
using UnityEngine;

public class DamageReceiverByType : DamageReceiver, IBurnable, IElectricable
{
    [Header("Damage Type")]
    [SerializeField] protected float exitTimeTwitch = 1f;
    [SerializeField] protected float exitTimeBurn = 2f;
    //[SerializeField] protected float exitTimeGlace = 4f;

    [SerializeField] protected int DamagePerSecondFire = 2;
    [SerializeField] protected int DamagePerSecondTwitch = 1;
    [SerializeField] protected int DamagePerSecondPoition = 1;


    [SerializeField] protected float TimeDurationFire = 3;
    [SerializeField] protected float TimeDurationGlace = 4;
    [SerializeField] protected float TimeDurationTwich = 2;
    [SerializeField] protected float TimeDurationPoition = 6;
    [SerializeField] protected float TimeDurationStun = 2;




    private Coroutine burnCoroutine = null;
    private Coroutine glaceCoroutine = null;
    private Coroutine twitchCoroutine = null;
    private Coroutine poitionCoroutine = null;
    private Coroutine stunCoroutine = null;


    private Transform objFX;
    #region Machine Effect

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //HandleCollisionEnter<IBurnable>(collision, DamagePerSecondFire, StartBurning);
        //HandleCollisionEnter<IElectricable>(collision, DamagePerSecondTwitch, StartTwitching);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        //HandleCollisionExit<IBurnable>(collision, exitTimeBurn, StopBurning);
        //HandleCollisionExit<IElectricable>(collision, exitTimeTwitch, StopTwitching);
    }

    protected virtual void HandleCollisionEnter<T>(Collider2D collision, int damagePerSecond, Action<int> startEffect)
    {
        var components = collision.transform.parent.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            if (component is T)
            {
                Debug.Log("Interface được tìm thấy");
                startEffect(damagePerSecond);
                break;
            }
        }
    }

    protected virtual void HandleCollisionExit<T>(Collider2D collision, float exitTime, Action stopEffect)
    {
        var components = collision.transform.parent.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            if (component is T)
            {
                StartCoroutine(CountDownAndStopEffect(exitTime, stopEffect));
                break;
            }
        }
    }

    private IEnumerator CountDownAndStopEffect(float time, Action stopEffect)
    {
        yield return new WaitForSeconds(time);
        stopEffect();
    }
    public void StartBurning(int damagePerSecond)
    {
        isBurning = true;

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isBurning));
    }
    public void StartTwitching(int damagePerSecond)
    {
        isTwitching = true;
        if (twitchCoroutine != null) StopCoroutine(twitchCoroutine);
        twitchCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isTwitching));

    }
    private IEnumerator ApplyEffect(int damagePerSecond, Func<bool> condition)
    {
        float interval = 1f / damagePerSecond;
        WaitForSeconds wait = new WaitForSeconds(interval);
        int damagePerTick = Mathf.CeilToInt(interval);

        while (condition())
        {
            yield return wait;
            DeductHealth(damagePerTick);
        }
    }
    #endregion
    #region STOP EFFECT
    public void StopBurning()
    {
        isBurning = false;
        if (burnCoroutine != null) StopCoroutine(burnCoroutine);

        objectCtrl.AbstractModel.EffectCharacter.SetMaterial(EffectManager.Instance.MaterialDefault);
    }
    public void StopGlace()
    {
        isGlacing = false;
        if (glaceCoroutine != null) StopCoroutine(glaceCoroutine);

        objectCtrl.AbstractModel.EffectCharacter.SetMaterial(EffectManager.Instance.MaterialDefault);
        enemyCtrl.ObjMovement.MoveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;  // Khôi phục vận tốc ban đầu
    }
    public void StopTwitching()
    {
        isTwitching = false;

        if (twitchCoroutine != null) StopCoroutine(twitchCoroutine);

        objectCtrl.AbstractModel.EffectCharacter.SetMaterial(EffectManager.Instance.MaterialDefault);
        this.enemyCtrl.ObjMovement.MoveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;
        this.enemyCtrl.AbstractModel.IsStun = false;


        Debug.Log("StopStun");
    }
    public void StopStun()
    {
        isStun = false;

        if (stunCoroutine != null) StopCoroutine(stunCoroutine);

        this.enemyCtrl.ObjMovement.MoveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;
        this.enemyCtrl.AbstractModel.IsStun = false;

        Debug.Log("StopStun");
    }
    public void StopPoition()
    {
        isPoition = false;
        if (poitionCoroutine != null) StopCoroutine(poitionCoroutine);

        objectCtrl.AbstractModel.EffectCharacter.SetMaterial(EffectManager.Instance.MaterialDefault);
    }
    #endregion
    #region Glace Effect
    public void StartGlace(SkillType skillType)
    {
        isGlacing = true;

        // Effect Character
        Material material = EffectManager.Instance.GetMaterialByName("glace");
        if (material != null)
        {
            Debug.Log("Set Glace Effect");
            objectCtrl.AbstractModel.EffectCharacter.SetMaterial(material);
        }

        if (enemyCtrl != null)
        {
            StartCoroutine(ReduceMoveSpeedTemporarily(enemyCtrl, 0.5f, TimeDurationGlace));
        }
    }

    private IEnumerator ReduceMoveSpeedTemporarily(EnemyCtrl enemyCtrl, float reductionFactor, float duration)
    {
        enemyCtrl.ObjMovement.MoveSpeed *= reductionFactor;  // Giảm vận tốc

        yield return new WaitForSeconds(duration);  // Chờ trong 4 giây

        StopGlace();
    }
    #endregion
    #region Burning Effect
    public void StartBurning(SkillType skillType)
    {
        isBurning = true;


        //Effect Character
        Material material = EffectManager.Instance.GetMaterialByName("fire");
        if(material!= null)
        {
            Debug.Log("Set Fire Burn");
            objectCtrl.AbstractModel.EffectCharacter.SetMaterial(material);
        }

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(ApplyBurningEffect(DamagePerSecondFire, TimeDurationFire,skillType));
    }
    private IEnumerator ApplyBurningEffect(int damagePerSecond, float duration, SkillType skillType)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        float elapsedTime = 0f;

        while (isBurning && elapsedTime < duration)
        {
            yield return wait;
            DeductHealth(damagePerSecond);
            Send(damagePerSecond, skillType);
            elapsedTime += 0.5f;
        }

        StopBurning(); // Dừng hiệu ứng burn khi hết thời gian
    }
    #endregion
    #region Electric Effect
    public void StartElectric(SkillType skillType)
    {
        isTwitching = true;

        // Effect Character
        Material material = EffectManager.Instance.GetMaterialByName("electric");
        if (material != null)
        {
            Debug.Log("Set Electric Effect");
            objectCtrl.AbstractModel.EffectCharacter.SetMaterial(material);
        }

        if (enemyCtrl != null)
        {
            Vector3 hitPos = transform.position;
            hitPos.y += 0.35f;

            objFX = FXSpawner.Instance.Spawn("Stun", hitPos, Quaternion.identity); // Assign to objFX

            objFX.GetComponentInChildren<FxDespawn>().delay = TimeDurationTwich;

            objFX.gameObject.SetActive(true); // Activate the GameObject

            StartCoroutine(ApplyTwitchingEffect(DamagePerSecondTwitch, TimeDurationTwich, skillType));

            this.enemyCtrl.AbstractModel.IsStun = true;
            this.enemyCtrl.ObjMovement.MoveSpeed = 0;
        }
    }

    private IEnumerator ApplyTwitchingEffect(int damagePerSecond, float duration, SkillType skillType)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        float elapsedTime = 0f;

        while (isTwitching && elapsedTime < duration)
        {
            yield return wait;
            DeductHealth(damagePerSecond);
            Send(damagePerSecond, skillType);
            elapsedTime += 0.5f;
        }

        StopTwitching(); // Dừng hiệu ứng twitching khi hết thời gian
    }
    #endregion
    #region Poition Effect
    public void StartPotioning(SkillType skillType)
    {
        isPoition = true;


        //Effect Character
        Material material = EffectManager.Instance.GetMaterialByName("poison");
        if (material != null)
        {
            Debug.Log("Set Fire Burn");
            objectCtrl.AbstractModel.EffectCharacter.SetMaterial(material);
        }

        if (poitionCoroutine != null) StopCoroutine(poitionCoroutine);
        poitionCoroutine = StartCoroutine(ApplyPoitioningEffect(DamagePerSecondPoition, TimeDurationPoition, skillType));
    }
    private IEnumerator ApplyPoitioningEffect(int damagePerSecond, float duration, SkillType skillType)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        float elapsedTime = 0f;

        while (isPoition && elapsedTime < duration)
        {
            yield return wait;
            DeductHealth(damagePerSecond);
            Send(damagePerSecond, skillType);
            elapsedTime += 0.5f;
        }

        StopPoition(); // Dừng hiệu ứng burn khi hết thời gian
    }
    #endregion
    #region StunEffect
    public void StartStun()
    {
        isStun = true;

        if (enemyCtrl != null)
        {
            Vector3 hitPos = transform.position;
            hitPos.y += 0.35f;

            objFX = FXSpawner.Instance.Spawn("Stun", hitPos, Quaternion.identity); // Assign to objFX

            objFX.GetComponentInChildren<FxDespawn>().delay = TimeDurationStun;

            objFX.gameObject.SetActive(true); // Activate the GameObject

            StartCoroutine(ApplyStunEffect(TimeDurationTwich));

            this.enemyCtrl.AbstractModel.IsStun = true;
            this.enemyCtrl.ObjMovement.MoveSpeed = 0;
        }
    }

    private IEnumerator ApplyStunEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        StopStun(); // Dừng hiệu ứng twitching khi hết thời gian
    }
    #endregion
    #region FX Text ... 
    public void Send(int dame, SkillType skillType)
    {
        Vector3 hitPos = transform.position;
        Quaternion hitRot = transform.rotation;

        //this.CreateImpactFX(hitPos, hitRot);
        this.CreateTextDamageFX(dame,hitPos, skillType);
    }
    protected virtual void CreateTextDamageFX(int dame,Vector3 hitPos, SkillType skillType)
    {
        string fxName = this.GetTextDamageFX();
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.DoAnimation(dame, skillType);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return FXSpawner.textDamage;
    }
    #endregion
    public override void OnDead()
    {
        Debug.Log("OnDead ");

        // Kiểm tra và cập nhật delay của objFX nếu đang tồn tại
        if (objFX != null)
        {
            var fxDespawn = objFX.GetComponentInChildren<FxDespawn>();
            if (fxDespawn != null)
            {
                fxDespawn.timer = fxDespawn.delay;
                Debug.Log("Set objFX delay to 0s");
            }
        }

        // Xử lý các hiệu ứng đang chạy
        HandleEffectCharacter();
    }
    protected void HandleEffectCharacter()
    {
        if (isBurning)
        {
            StopBurning();
            Debug.Log("Stop Burn");
        }
        if(isGlacing)
        {
            StopGlace();
            Debug.Log("Stop Glacing");

        }
        if (isTwitching)
        {
            StopTwitching();
            Debug.Log("Stop Twitching");
        }
        if(isPoition)
        {
            StopPoition();
            Debug.Log("Stop Poitioning");
        }
        if(isStun)
        {
            StopStun();
            Debug.Log("Stop Stun");
        }
    }
}
