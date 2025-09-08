using System;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public static event Action OnMaxEnemySpawn;
    public static event Action OnWaveOver;

    [SerializeField] private string enemyTag = "Enemy"; // Enemy prefab tag
    [SerializeField] private int maxEnemyAmount = 1;

    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemiesKilledThisWave = 0;
    [SerializeField] private int enemiesToKillThisWave = 1;

    private bool waveActive = true;

    void Update()
    {
        int currentEnemyCount = GameObject.FindGameObjectsWithTag(enemyTag).Length;

        if (currentEnemyCount >= maxEnemyAmount)
            OnMaxEnemySpawn?.Invoke();

        maxEnemyAmount = enemiesToKillThisWave;

        if (waveActive && enemiesKilledThisWave >= enemiesToKillThisWave)
        {
            EndWave();
        }
    }

    public void EnemyKilled()
    {
        if (!waveActive) return;

        enemiesKilledThisWave++;
        Debug.Log($"Enemy killed. Total killed this wave: {enemiesKilledThisWave}");
    }

    private void EndWave()
    {
        waveActive = false;
        Debug.Log($"Wave {waveNumber} ended!");
        OnWaveOver?.Invoke();
        Invoke(nameof(StartNextWave), 5f);
    }

    private void StartNextWave()
    {
        waveNumber++;
        enemiesKilledThisWave = 0;
        enemiesToKillThisWave += 5;
        waveActive = true;
        Debug.Log($"Wave {waveNumber} started! (Level {GetLevel()})");
    }

    // Public getters
    public int GetWaveNumber() => waveNumber;
    public bool IsWaveActive() => waveActive;
    public int GetEnemiesToKillThisWave() => enemiesToKillThisWave;
    public int GetCurrentEnemiesInScene() => GameObject.FindGameObjectsWithTag(enemyTag).Length;

    public float GetLevel() => Mathf.Clamp(1 + ((waveNumber - 1) / 10), 1, 3);
}
