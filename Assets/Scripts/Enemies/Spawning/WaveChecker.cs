using System;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public static event Action OnMaxEnemySpawn;
    public static event Action OnWaveOver;
    public static event Action OnBossWave;
    public static event Action<int> On10WavesCompleted;  // now sends wave number

    [Header("Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private int enemiesToKillThisWave = 1;

    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField] private bool waveActive = true;

    [SerializeField] private int bossEnemyAmount = 0;
    [SerializeField] private int enemiesspawned;
    public int BossEnemyAmount => bossEnemyAmount;
    public int GetCurrentWaveNumber() => waveNumber;


    private void Start()
    {
        EnemySpawner.OnEnemySpawn += HandleEnemySpawn;
    }

    private void Update()
    {
        if (enemiesspawned >= enemiesToKillThisWave)
        {
            MaxEnemySpawn();
        }

        if (waveActive && enemiesKilled >= enemiesToKillThisWave)
            EndWave();
    }

    private void HandleEnemySpawn()
    {
        enemiesspawned++;
        Debug.Log($"[WaveChecker] Enemy spawned. {enemiesspawned}/{enemiesToKillThisWave}");
    }

    private void MaxEnemySpawn()
    {
        OnMaxEnemySpawn?.Invoke();
    }

    public void EnemyKilled()
    {
        if (!waveActive) return;
        enemiesKilled++;
        Debug.Log($"[WaveChecker] EnemyKilled received. {enemiesKilled}/{enemiesToKillThisWave}");
    }

    public void EnemyReachedEnd()
    {
        if (!waveActive) return;
        enemiesKilled++;
        Debug.Log($"[WaveChecker] EnemyReachedEnd received. {enemiesKilled}/{enemiesToKillThisWave}");
    }

    private void EndWave()
    {
        waveActive = false;
        Debug.Log($"[WaveChecker] Wave {waveNumber} ended.");
        OnWaveOver?.Invoke();
        Invoke(nameof(StartNextWave), 5f);
        enemiesToKillThisWave++;
        CurrencyManager.Instance.AddCurrency(waveNumber * 10);
    }

    private void StartNextWave()
    {
        waveNumber++;
        enemiesKilled = 0;
        enemiesspawned = 0; // reset for next wave
        enemiesToKillThisWave += 5;
        waveActive = true;

        Debug.Log($"[WaveChecker] Wave {waveNumber} started. Kill {enemiesToKillThisWave} enemies.");

        if (waveNumber % 10 == 0)
        {
            Debug.Log("[WaveChecker] Boss Wave Reached!");
            waveActive = false; // stop normal spawning
            bossEnemyAmount++;
            OnBossWave?.Invoke();
            On10WavesCompleted?.Invoke(waveNumber); // send wave number
        }
    }

    private void OnEnable()
    {
        EnemyEvents.OnEnemyKilled += EnemyKilled;
        EnemyEvents.OnEnemyReachedEnd += EnemyReachedEnd;
        EnemyEvents.OnBossDefeated += HandleBossDefeat;
    }

    private void OnDisable()
    {
        EnemyEvents.OnEnemyKilled -= EnemyKilled;
        EnemyEvents.OnEnemyReachedEnd -= EnemyReachedEnd;
        EnemyEvents.OnBossDefeated -= HandleBossDefeat;
        EnemySpawner.OnEnemySpawn -= HandleEnemySpawn;
    }

    private void HandleBossDefeat()
    {
        Debug.Log("[WaveChecker] Boss defeated! Starting next wave...");
        Invoke(nameof(StartNextWave), 5f);
    }

    // Public Getters
    public int GetWaveNumber() => waveNumber;
    public bool IsWaveActive() => waveActive;
    public int GetEnemiesToKillThisWave() => enemiesToKillThisWave;
    public int GetCurrentEnemiesInScene() => GameObject.FindGameObjectsWithTag(enemyTag).Length;
}
