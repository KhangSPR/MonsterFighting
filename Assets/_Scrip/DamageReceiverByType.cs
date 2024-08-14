// Derived class handling specific damage types
using System;
using System.Collections;
using UnityEngine;

public class DamageReceiverByType : DamageReceiver, IBurnable, IElectricable
{
    [Header("Damage Type")]
    [SerializeField] protected float exitTimeTwitch = 1f;
    [SerializeField] protected float exitTimeBurn = 2f;

    [SerializeField] protected int DamagePerSecondFire = 2;
    [SerializeField] protected int DamagePerSecondTwitch = 2;

    [SerializeField] protected int TimeDurationFire = 3;

    private bool isBurning;
    private bool isTwitching;

    private Coroutine burnCoroutine;
    private Coroutine twitchCoroutine;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollisionEnter<IBurnable>(collision, DamagePerSecondFire, StartBurning);
        HandleCollisionEnter<IElectricable>(collision, DamagePerSecondTwitch, StartTwitching);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        HandleCollisionExit<IBurnable>(collision, exitTimeBurn, StopBurning);
        HandleCollisionExit<IElectricable>(collision, exitTimeTwitch, StopTwitching);
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

    public void StopBurning()
    {
        isBurning = false;
        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
    }

    public void StartTwitching(int damagePerSecond)
    {
        isTwitching = true;
        if (twitchCoroutine != null) StopCoroutine(twitchCoroutine);
        twitchCoroutine = StartCoroutine(ApplyEffect(damagePerSecond, () => isTwitching));
    }

    public void StopTwitching()
    {
        isTwitching = false;
        if (twitchCoroutine != null) StopCoroutine(twitchCoroutine);
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
    public void StartBurning()
    {
        isBurning = true;
        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(ApplyBurningEffect(DamagePerSecondFire, TimeDurationFire));
    }

    //private IEnumerator ApplyBurningEffect(int damagePerSecond, float duration)
    //{
    //    float interval = 1f / damagePerSecond;
    //    WaitForSeconds wait = new WaitForSeconds(interval);
    //    int damagePerTick = Mathf.CeilToInt(interval);

    //    float elapsedTime = 0f;

    //    while (isBurning && elapsedTime < duration)
    //    {
    //        yield return wait;
    //        DeductHealth(damagePerTick);
    //        Send(damagePerTick);
    //        elapsedTime += interval;
    //    }

    //    StopBurning(); // Dừng hiệu ứng burn khi hết thời gian
    //}
    private IEnumerator ApplyBurningEffect(int damagePerSecond, float duration)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        float elapsedTime = 0f;

        while (isBurning && elapsedTime < duration)
        {
            yield return wait;
            DeductHealth(damagePerSecond);
            Send(damagePerSecond);
            elapsedTime += 0.5f;
        }

        StopBurning(); // Dừng hiệu ứng burn khi hết thời gian
    }
    public override void OnDead()
    {
        // Implement specific death logic here
    }
    public void Send(int dame)
    {
        Vector3 hitPos = transform.position;
        Quaternion hitRot = transform.rotation;

        //this.CreateImpactFX(hitPos, hitRot);
        this.CreateTextDamageFX(dame,hitPos);
    }
    protected virtual void CreateTextDamageFX(int dame,Vector3 hitPos)
    {
        string fxName = this.GetTextDamageFX();
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.SetDamage(dame);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return FXSpawner.textDamage;
    }
}
