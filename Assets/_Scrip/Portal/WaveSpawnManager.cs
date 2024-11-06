using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIGameDataMap;
using UnityEngine;

public class WaveSpawnManager : SaiMonoBehaviour
{
    private static WaveSpawnManager instance;
    public static WaveSpawnManager Instance => instance;

    [Header("Portal Spawner")]
    [SerializeField] protected PortalSpawnAction portalSpawnAction;
    public PortalSpawnAction PortalSpawnAction => portalSpawnAction;
    [SerializeField] protected ProgressPortals progressPortals;
    public ProgressPortals ProgressPortals => progressPortals;
    [SerializeField] protected PortalTimer portalTimer;
    public PortalTimer PortalTimer => portalTimer;
    [SerializeField] protected EnemySpawnCtrl enemySpawnCtrl;
    public EnemySpawnCtrl EnemySpawnCtrl => enemySpawnCtrl;
    [SerializeField] protected SpawnPoints spawnPoints;
    public SpawnPoints SpawnPoints => spawnPoints;

    [Header("ListPortals Wave")]
    public List<Portals> portalsWave = new List<Portals>();
    [Header("ListPortals Spawning")]
    public List<Portals> portalsSpawning = new List<Portals>();


    [SerializeField] private MapSO mapSO;
    public MapSO MapSO
    {
        get { return mapSO; }
        set { mapSO = value; }
    }

    private Difficult difficult;
    public Difficult Difficult
    {
        get { return difficult; }
        set { difficult = value; }
    }
    private Wave[] waves;
    public Wave[] Wave
    {
        get { return waves; }
        set { waves = value; }
    }
    [SerializeField]
    private Wave currentWave;
    public Wave CurrentWave
    {
        get { return currentWave; }
        set { currentWave = value; }
    }

    [SerializeField]
    private bool portalsSpawnType; // Marks when portalsSpawning has finished spawning
    //[SerializeField]
    private List<AbilitySummon> abilitySummons = new List<AbilitySummon>();
    public List<AbilitySummon> AbilitySummons
    {
        get { return abilitySummons; }
        set { abilitySummons = value; }
    }

    // Events
    public static event Action AllPortalsSpawned;
    public static event Action<Wave, bool> UpdateWave;

    // Animation
    [SerializeField] TextSizeAnimation textSizeAnimation;
    [SerializeField] TMP_Text text_Wave;
    const float k_LerpTime = 0.6f;
    [SerializeField]
    private int currentWaveIndex;

    public int CurrentWaveIndex
    {
        get { return currentWaveIndex; }
        set
        {
            if (currentWaveIndex != value)
            {
                StartCoroutine(LerpRoutine(text_Wave, currentWaveIndex, value, k_LerpTime));
                currentWaveIndex = value;
            }
        }
    }

    // Coroutine để animate số hiệu sóng
    IEnumerator LerpRoutine(TMP_Text label, int startValue, int endValue, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            label.text = $"<color=green>{currentValue}</color>/{waves.Length}";
            yield return null;
        }
        // Đảm bảo giá trị cuối cùng được thiết lập chính xác
        label.text = $"<color=green>{endValue}</color>/{waves.Length}";
    }
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        if (mapSO == null) return;

        // Initialize the first wave
        UpdateFirstWave();

        InvokeRepeating(nameof(LoadWaveTypeCurrentNext), 2f, 1f);


        //Update Text Wave
        text_Wave.text = "<color=green>" + currentWaveIndex.ToString() + "</color>/" + waves.Length.ToString();
    }

    public void UpdateFirstWave()
    {
        // Update First
        currentWave = Wave[0];
        LoadCurrentSpawningPortal();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAllComponents();
    }

    private void LoadAllComponents()
    {
        portalSpawnAction = portalSpawnAction ?? GetComponentInChildren<PortalSpawnAction>();
        portalTimer = portalTimer ?? GetComponentInChildren<PortalTimer>();
        progressPortals = progressPortals ?? GetComponentInChildren<ProgressPortals>();
        enemySpawnCtrl = enemySpawnCtrl ?? GetComponentInChildren<EnemySpawnCtrl>();
        spawnPoints = spawnPoints ?? GetComponentInChildren<SpawnPoints>();

        Debug.Log($"{gameObject.name}: Loaded all components");
    }
    private void LoadCurrentSpawningPortal()
    {
        // Clear Current Spawning
        portalsSpawning.Clear();

        // Assign the spawning portals to the list
        portalsSpawning = mapSO.GetPortalsSpawning(waves[currentWaveIndex]).ToList();

        //No Portal Wave
        if (portalsSpawning.Count > 0)
        {
            hasPortalAbilitySummons = true;
        }
        else
        {
            hasPortalAbilitySummons = false;
        }

        // Update spawn timers or other related actions
        portalTimer.UpdateTimeSpawns(false);

        // Invoke UpdateWave event
        UpdateWave?.Invoke(currentWave, false);

        // Update the portal spawn action
        this.portalSpawnAction.PortalSpawns = portalsSpawning.ToArray();

        
        // Set the flag to indicate spawning portals have been loaded
        // Reset the portalsSpawnType if necessary
    }
    private void LoadCurrentWavePortals()
    {
        GameManager.Instance.ReadyTimer = false;
        // Clear Current Wave
        portalsWave.Clear();

        // Assign the portals to the list
        portalsWave = mapSO.GetPortalsWave(waves[currentWaveIndex]).ToList();

        //No Portal Wave

        // Update the portal spawn action
        this.portalSpawnAction.PortalSpawns = portalsWave.ToArray();

        // After the animation ends, update the spawn time of portals
        portalTimer.UpdateTimeSpawns(true);

        // Update Wave
        UpdateWave?.Invoke(currentWave, true);

        // Start any required animations or additional logic
        StartCoroutine(WaitForTextAnimation());
    }

    private IEnumerator WaitForTextAnimation()
    {
        Debug.Log("Bắt đầu coroutine: WaitForTextAnimation");

        textSizeAnimation.gameObject.SetActive(true);
        

        if (textSizeAnimation != null)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Đã đợi 1 giây");

            textSizeAnimation.PlayWaveAnimation("A HUGE WAVE OF MONSTERS IS APPROACHING!", false);
            Debug.Log("Đã gọi PlayWaveAnimation");

            yield return new WaitForSeconds(5f);
            Debug.Log("Đã đợi 5 giây");

            if (currentWaveIndex == waves.Length - 1)
            {
                textSizeAnimation.PlayWaveAnimation("FINAL WAVE", true);
                Debug.Log("Đã gọi PlayWaveAnimation cho FINAL WAVE");

                yield return new WaitForSeconds(2f);
                Debug.Log("Đã đợi 2 giây cho FINAL WAVE");
            }
        }

        textSizeAnimation.textUI.text = "";
        textSizeAnimation.gameObject.SetActive(false);

        CurrentWaveIndex++;

        GameManager.Instance.ReadyTimer = true;
        Debug.Log("Đã đặt ReadyTimer thành true");
    }


    /// <summary>
    /// ///////////////////////////////////CHECK WIN WAVE SPAWANING
    /// </summary>
    ///
    [SerializeField]
    private bool checkOneWave = false;
    private bool stopLoadingWaves = false;
    [SerializeField] bool hasPortalAbilitySummons = false;
    public bool IsFinalWave()
    {
        return currentWaveIndex == waves.Length-1;
    }
    private void WaveFinallyGame()
    {
        if (checkOneWave  || IsFinalWave() && abilitySummons.Count <0)
        {
            StartCoroutine(InvokeAllPortalsSpawnedAfterDelay(2f));

            stopLoadingWaves = true;  //= LoadWaveTypeCurrentNext
            return;
        }
    }
    private IEnumerator InvokeAllPortalsSpawnedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  
        AllPortalsSpawned?.Invoke(); 
    }

    public void LoadPortalsWaveComplete()
    {
        //Point Empty == false ALL
        this.spawnPoints.ResetPointEmpty();

        Debug.Log("Wave Lenght = " + waves.Length);
        if (currentWaveIndex < waves.Length - 1) // All enemies in current wave defeated
        {
            Debug.Log("CheckWave");

            // Update current wave
            currentWave = waves[currentWaveIndex];

            // Load the next wave's spawning portals
            LoadCurrentWavePortals();

        }
        else if (IsFinalWave())
        {
            // If this is the final wave
            LoadCurrentWavePortals();

            // Reset flags
            checkOneWave = true;
        }
    }

    public void LoadWaveTypeCurrentNext()
    {
        bool abilitySummonPortal = OnALLEnemysSpawnedPortal();
        bool abilitySummonEnemy = OnALLEnemysSpawned();

        Debug.Log("Load abilitySummonPortal: " + abilitySummonPortal);
        Debug.Log("Load abilitySummonEnemy: " + abilitySummonEnemy);

        if (abilitySummonPortal && abilitySummonEnemy)
        {
            WaveFinallyGame();

            if (stopLoadingWaves) return;


            if (!portalsSpawnType)
            {
                LoadPortalsWaveComplete();
                Debug.Log("Wave Complete");
            }
            else
            {
                // Update current wave
                currentWave = waves[currentWaveIndex];

                LoadCurrentSpawningPortal();

                Debug.Log("Spawning Complete");
            }
            if(abilitySummons != null)
            {
                abilitySummons.Clear();
            }

            portalsSpawnType = !portalsSpawnType;

        }
    }

    private bool OnALLEnemysSpawnedPortal()
    {
        //if (abilitySummons == null)
        //{
        //    Debug.Log("abilitySummons is null, returning true.");
        //    return false;
        //}
        //Debug.Log($"abilitySummons count: {abilitySummons.Count}");

        //if (abilitySummons.Count <= 0)
        //{
        //    Debug.Log("abilitySummons count is 0 or less, returning false.");
        //    return false;
        //}

        foreach (var ability in abilitySummons)
        {
            Debug.Log($"Checking ability: {ability}");

            if (!ability.CheckAllEnemyDead)
            {
                Debug.Log("An enemy is not dead, returning false.");
                return false;
            }
        }
        if(!hasPortalAbilitySummons)
        {
            return true;
        }

        Debug.Log("All enemies are dead, returning true.");
        return true;
    }


    private bool OnALLEnemysSpawned()
    {
        AbilitySummonEnemy abilitySummonEnemy = (AbilitySummonEnemy)this.enemySpawnCtrl.Abilities.AbilitySummon;

        if (abilitySummonEnemy == null) return false;

        bool isEnemyDead = abilitySummonEnemy.CheckAllEnemyDead;

        //if (isEnemyDead)
        //{
        //    abilitySummonEnemy.UpdateClear();

        //    Debug.Log("Clear");
        //}
        return isEnemyDead;
    }
}
