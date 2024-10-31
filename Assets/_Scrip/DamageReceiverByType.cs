// Derived class handling specific damage types
using System;
using System.Collections;
using UIGameDataMap;
using Unity.VisualScripting;
using UnityEngine;

public class DamageReceiverByType : DamageReceiver, IBurnable, IElectricable, IDarkable, IPotionable
{
    [Header("Damage Type")]
    [SerializeField] protected float exitTimeTwitch = 1f;
    [SerializeField] protected float exitTimeBurn = 2f;
    [SerializeField] protected float exitTimeDark = 2f;
    //[SerializeField] protected float exitTimeGlace = 4f;

    [SerializeField] protected int DamagePerSecondFire = 2;
    [SerializeField] protected int DamagePerSecondTwitch = 1;
    [SerializeField] protected int DamagePerSecondPoition = 1;
    [SerializeField] protected int DamagePerSecondDark = 1;


    [SerializeField] protected float TimeDurationFire = 3;
    [SerializeField] protected float TimeDurationGlace = 4;
    [SerializeField] protected float TimeDurationTwich = 2;
    [SerializeField] protected float TimeDurationPoition = 6;
    [SerializeField] protected float TimeDurationStun = 2;
    [SerializeField] protected float TimeDurationDark = 3;




    private Coroutine burnCoroutine = null;
    private Coroutine glaceCoroutine = null;
    private Coroutine twitchCoroutine = null;
    private Coroutine poitionCoroutine = null;
    private Coroutine stunCoroutine = null;
    private Coroutine darkCoroutine = null;


    #region Machine Effect

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollisionEnter<IDarkable>(collision, DamagePerSecondDark, StartDarking);
        //HandleCollisionEnter<IElectricable>(collision, DamagePerSecondTwitch, StartTwitching);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        HandleCollisionExit<IDarkable>(collision, exitTimeDark, StopDarking);
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
                //StartCoroutine(CountDownAndStopEffect(exitTime, stopEffect));
                CoroutineManager.Instance.StartGlobalCoroutine(CountDownAndStopEffect(exitTime, stopEffect));
                break;
            }
        }
    }

    private IEnumerator CountDownAndStopEffect(float time, Action stopEffect)
    {
        yield return new WaitForSeconds(time);
        stopEffect();
    }
        private IEnumerator ApplyEffect(int damagePerSecond, Func<bool> condition, SkillType skillType)
    {
        float interval = 1f / damagePerSecond;
        WaitForSeconds wait = new WaitForSeconds(interval);
        int damagePerTick = Mathf.CeilToInt(interval);

        while (condition())
        {
            yield return wait;
            DeductHealth(damagePerTick);
            FXSpawner.Instance.SendFXText(damagePerSecond, skillType, transform, Quaternion.identity);

            Debug.Log("Darking");
        }
    }
    public void StartBurning(int damagePerSecond)
    {
        isBurning = true;

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isBurning, SkillType.Fire));
    }
    public void StartTwitching(int damagePerSecond)
    {
        isTwitching = true;
        if (twitchCoroutine != null) StopCoroutine(twitchCoroutine);
        twitchCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isTwitching, SkillType.Electric));

    }
    #endregion
    #region Potioning Effect
    public void StartPotioning(int damagePerSecond)
    {
        if (this.enemyCtrl != null) return; //Repair

        // Nếu darkCoroutine đang chạy, dừng nó trước khi khởi động lại
        if (poitionCoroutine != null)
        {
            StopCoroutine(poitionCoroutine);
            poitionCoroutine = null;
        }

        isPoition = true;

        // Khởi động lại hiệu ứng Stun từ đầu
        StartStun();

        // Bắt đầu lại hiệu ứng Darking
        poitionCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isPoition, SkillType.Poison));
    }

    public void StopPotioning()
    {
        // Đảm bảo không chạy tiếp nếu object đã bị hủy
        if (this == null || transform == null || transform.parent == null)
        {
            Debug.LogWarning("StopDarking: Đối tượng đã bị hủy, không thể tiếp tục thực thi.");
            return;
        }

        isPoition = false;

        // Kiểm tra darkCoroutine có tồn tại và dừng coroutine
        if (poitionCoroutine != null)
        {
            // Kiểm tra nếu đối tượng cha đã bị vô hiệu hóa
            if (!transform.parent.gameObject.activeSelf) return;

            StopCoroutine(poitionCoroutine);
            poitionCoroutine = null; // Đảm bảo đặt về null sau khi dừng
        }


        // Thay đổi material nếu tất cả thành phần đều tồn tại
        this.AbstractModel.DameFlash.SetMaterialDamageFlash();
    }
    #endregion
    #region Darking Effect
    public void StartDarking(int damagePerSecond)
    {
        if (this.playerCtrl != null) return; //Repair

        // Nếu darkCoroutine đang chạy, dừng nó trước khi khởi động lại
        if (darkCoroutine != null)
        {
            StopCoroutine(darkCoroutine);
            darkCoroutine = null;
        }

        isDarking = true;

        // Khởi động lại hiệu ứng Stun từ đầu
        StartStun();

        // Bắt đầu lại hiệu ứng Darking
        darkCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isDarking, SkillType.Dark));
    }


    public void StopDarking()
    {
        // Đảm bảo không chạy tiếp nếu object đã bị hủy
        if (this == null || transform == null || transform.parent == null)
        {
            Debug.LogWarning("StopDarking: Đối tượng đã bị hủy, không thể tiếp tục thực thi.");
            return;
        }

        isDarking = false;

        // Kiểm tra darkCoroutine có tồn tại và dừng coroutine
        if (darkCoroutine != null)
        {
            // Kiểm tra nếu đối tượng cha đã bị vô hiệu hóa
            if (!transform.parent.gameObject.activeSelf) return;

            StopCoroutine(darkCoroutine);
            darkCoroutine = null; // Đảm bảo đặt về null sau khi dừng
        }
        // Thay đổi material nếu tất cả thành phần đều tồn tại
        this.AbstractModel.DameFlash.SetMaterialDamageFlash();
    }



    #endregion
    #region STOP EFFECT
    public void StopBurning()
    {
        isBurning = false;
        if (burnCoroutine != null) StopCoroutine(burnCoroutine);

        this.AbstractModel.DameFlash.SetMaterialDamageFlash();
    }
    public void StopGlace()
    {
        isGlacing = false;
        if (glaceCoroutine != null) StopCoroutine(glaceCoroutine);

        this.AbstractModel.DameFlash.SetMaterialDamageFlash();
        enemyCtrl.ObjMovement.MoveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;  // Khôi phục vận tốc ban đầu
    }
    public void StopTwitching()
    {
        isTwitching = false;

        if (twitchCoroutine != null) StopCoroutine(twitchCoroutine);

        this.AbstractModel.DameFlash.SetMaterialDamageFlash();
        this.enemyCtrl.ObjMovement.MoveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;
        this.enemyCtrl.AbstractModel.IsStun = false;


        Debug.Log("StopStun");
    }
    public void StopPoition()
    {
        isPoition = false;
        if (poitionCoroutine != null) StopCoroutine(poitionCoroutine);

        this.AbstractModel.DameFlash.SetMaterialDamageFlash();
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
            this.AbstractModel.EffectCharacter.SetMaterial(material);
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
            this.AbstractModel.EffectCharacter.SetMaterial(material);
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
            FXSpawner.Instance.SendFXText(damagePerSecond, skillType,transform,Quaternion.identity);
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
            this.AbstractModel.EffectCharacter.SetMaterial(material);
        }

        if (enemyCtrl != null)
        {
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
            FXSpawner.Instance.SendFXText(damagePerSecond, skillType, transform, Quaternion.identity);
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
            this.AbstractModel.EffectCharacter.SetMaterial(material);
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
            if(playerCtrl!=null)
            {
                FXSpawner.Instance.SendFXText(damagePerSecond, skillType, playerCtrl.TargetPosition, Quaternion.identity);
            }
            else
            {
                FXSpawner.Instance.SendFXText(damagePerSecond, skillType, transform, Quaternion.identity);
            }
            elapsedTime += 0.5f;
        }

        StopPoition(); // Dừng hiệu ứng burn khi hết thời gian
    }
    #endregion
    #region StunEffect
    public void StartStun()
    {
        isStun = true;

        // Nếu stunCoroutine đang chạy, dừng nó trước khi khởi động lại
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
            stunCoroutine = null;
        }

        if (enemyCtrl != null)
        {
            stunCoroutine = StartCoroutine(ApplyStunEffect(TimeDurationStun)); // Khởi động lại từ đầu

            this.enemyCtrl.AbstractModel.IsStun = true;
            this.enemyCtrl.ObjMovement.MoveSpeed = 0;

            Debug.Log("Call Start Stun");
        }
        else if(playerCtrl!=null)
        {
            stunCoroutine = StartCoroutine(ApplyStunEffect(TimeDurationStun)); // Khởi động lại từ đầu

            this.playerCtrl.AbstractModel.IsStun = true;
            Debug.Log("Call Start Stun");
        }
    }


    private IEnumerator ApplyStunEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        StopStun(); // Dừng hiệu ứng twitching khi hết thời gian
    }
    public void StopStun()
    {
        isStun = false;

        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);

        if (enemyCtrl != null)
        {
            if (enemyCtrl.ObjMovement != null)
                this.enemyCtrl.ObjMovement.MoveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;

            if (enemyCtrl.AbstractModel != null)
                this.enemyCtrl.AbstractModel.IsStun = false;

            Debug.Log("StopStun");
        }
        else if (playerCtrl!= null)
        {
            if (playerCtrl.AbstractModel != null)
                this.playerCtrl.AbstractModel.IsStun = false;

            Debug.Log("StopStun");
        }
        //else
        //{
        //    Debug.LogError("enemyCtrl is null in StopStun!" + transform.parent.name);
        //}
    }

    #endregion
    public override void OnDead()
    {
        Debug.Log("OnDead ");

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
        if(isDarking)
        {
            StopDarking();
            Debug.Log("Stop Darking");
        }
    }

}
