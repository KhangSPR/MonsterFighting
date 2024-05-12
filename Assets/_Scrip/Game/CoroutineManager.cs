using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineManager : SaiMonoBehaviour
{
    private static CoroutineManager instance;
    public static CoroutineManager Instance => instance;

    private List<Coroutine> runningCoroutines = new List<Coroutine>();

    protected override void Awake()
    {
        base.Awake();
        //if (CoroutineManager.instance != null) Debug.LogError("Only 1 CardManager Warning");
        CoroutineManager.instance = this;
    }

    // Đăng ký một Coroutine và thêm nó vào danh sách đang chạy
    public Coroutine StartManagedCoroutine(IEnumerator routine)
    {
        Coroutine coroutine = StartCoroutine(routine);
        runningCoroutines.Add(coroutine);
        return coroutine;
    }
    public void StopManagedCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            runningCoroutines.Remove(coroutine);
        }
    }

    // Cập nhật danh sách các Coroutine
    protected override void Update()
    {
        base.Update();
        for (int i = runningCoroutines.Count - 1; i >= 0; i--)
        {
            if (runningCoroutines[i] == null)
            {
                runningCoroutines.RemoveAt(i);
            }
        }
    }
}
