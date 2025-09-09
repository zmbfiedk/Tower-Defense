using System;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public static event Action OnMaxEnemySpawn;
    public static event Action OnWaveOver;

    [Header("Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private int enemiesToKillThisWave = 1;

    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField]private bool waveActive = true;

    private void Update()
    {
        int currentEnemies = GameObject.FindGameObjectsWithTag(enemyTag).Length;

        if (currentEnemies >= enemiesToKillThisWave)
            OnMaxEnemySpawn?.Invoke();

        if (waveActive && enemiesKilled >= enemiesToKillThisWave)
            EndWave();
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
    }

    private void StartNextWave()
    {
        waveNumber++;
        enemiesKilled = 0;
        enemiesToKillThisWave += 5;
        waveActive = true;
        Debug.Log($"[WaveChecker] Wave {waveNumber} started. Kill {enemiesToKillThisWave} enemies.");
    }

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += EnemyKilled;
        Enemy.OnEnemyReachedEnd += EnemyReachedEnd; 
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= EnemyKilled;
        Enemy.OnEnemyReachedEnd -= EnemyReachedEnd;
    }

    // Public Getters
    public int GetWaveNumber() => waveNumber;
    public bool IsWaveActive() => waveActive;
    public int GetEnemiesToKillThisWave() => enemiesToKillThisWave;
    public int GetCurrentEnemiesInScene() => GameObject.FindGameObjectsWithTag(enemyTag).Length;
}
