using System;
using System.Collections.Generic;
using UnityEngine;

public struct DifficultyState
{
    public float healthMultiplier;
    public float speedMultiplier;
    public float damageMultiplier;
    public float spawnRateMultiplier;
}

[DefaultExecutionOrder(-50)]
public class WaveDifficultyManager : MonoBehaviour
{
    public static WaveDifficultyManager Instance { get; private set; }

    // Broadcast when difficulty changes
    public static event Action<DifficultyState> OnDifficultyChanged;

    [Header("Milestones (ordered)")]
    [SerializeField] private List<WaveMilestone> milestones = new List<WaveMilestone>();

    private DifficultyState currentState;

    // internal multipliers start at 1
    private float accumulatedHealth = 1f;
    private float accumulatedSpeed = 1f;
    private float accumulatedDamage = 1f;
    private float accumulatedSpawnRate = 1f;

    private EnemySpawner spawner;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        currentState = new DifficultyState
        {
            healthMultiplier = 1f,
            speedMultiplier = 1f,
            damageMultiplier = 1f,
            spawnRateMultiplier = 1f
        };

        spawner = FindObjectOfType<EnemySpawner>();
    }

    private void OnEnable()
    {
        WaveChecker.On10WavesCompleted += Handle10WavesCompleted;
    }

    private void OnDisable()
    {
        WaveChecker.On10WavesCompleted -= Handle10WavesCompleted;
    }

    private void Handle10WavesCompleted(int waveNumber)
    {
        // Find milestones exactly matching the triggered waveNumber
        foreach (var m in milestones)
        {
            if (m == null) continue;
            if (m.waveNumber == waveNumber)
                ApplyMilestone(m);
        }
    }

    private void ApplyMilestone(WaveMilestone m)
    {
        accumulatedHealth *= m.healthMultiplier;
        accumulatedSpeed *= m.speedMultiplier;
        accumulatedDamage *= m.damageMultiplier;
        accumulatedSpawnRate *= m.spawnRateMultiplier;

        currentState.healthMultiplier = accumulatedHealth;
        currentState.speedMultiplier = accumulatedSpeed;
        currentState.damageMultiplier = accumulatedDamage;
        currentState.spawnRateMultiplier = accumulatedSpawnRate;

        OnDifficultyChanged?.Invoke(currentState);

        if (spawner != null)
        {

            spawner.SetSpawnTimeMultiplier(currentState.spawnRateMultiplier);

            if (m.unlockHealer) spawner.UnlockHealer();
            if (m.unlockInvisible) spawner.UnlockInvisible();
            if (m.unlockNewEnemy && m.newEnemyPrefab != null) spawner.AddEnemyPrefabToPool(m.newEnemyPrefab);
        }

        Debug.Log($"[WaveDifficultyManager] Applied milestone for wave {m.waveNumber}. " +
                  $"H:{currentState.healthMultiplier} S:{currentState.speedMultiplier} D:{currentState.damageMultiplier} SR:{currentState.spawnRateMultiplier}");
    }

    // Public getter for manual queries
    public DifficultyState GetCurrentState() => currentState;
}
