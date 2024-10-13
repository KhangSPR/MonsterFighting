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

    // Dừng và xóa Coroutine ra khỏi danh sách
    public void StopManagedCoroutine(Coroutine coroutine)
    {
        if (coroutine != null && runningCoroutines.Contains(coroutine))
        {
            StopCoroutine(coroutine);
            runningCoroutines.Remove(coroutine);
        }
    }

    // Xóa tất cả coroutine đang chạy nếu cần thiết
    public void StopAllManagedCoroutines()
    {
        foreach (var coroutine in runningCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        runningCoroutines.Clear();
    }

    ///
    public void StartGlobalCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
