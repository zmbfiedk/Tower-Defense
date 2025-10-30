using System;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class WaveDifficultyManager : MonoBehaviour
{
    public struct DifficultyState
    {
        public float healthMultiplier;
        public float speedMultiplier;
        public float damageMultiplier;
        public float spawnRateMultiplier;
    }

    // Singleton instance
    public static WaveDifficultyManager Instance { get; private set; }

    // Event triggered when difficulty changes
    public static event Action<DifficultyState> OnDifficultyChanged;

    [Header("Base Scaling Rates")]
    [SerializeField] private float waveGrowthRate = 0.05f;       // 5% harder each wave
    [SerializeField] private float distanceEaseFactor = 0.05f;   // 5% easier if enemies reach end
    [SerializeField] private float distanceHardFactor = 0.05f;   // 5% harder if they die early

    [Header("Clamp Values")]
    [SerializeField] private float maxMultiplier = 5f;
    [SerializeField] private float minMultiplier = 0.5f;

    private DifficultyState currentState;

    private float accumulatedHealth = 1f;
    private float accumulatedSpeed = 1f;
    private float accumulatedDamage = 1f;
    private float accumulatedSpawnRate = 1f;

    private EnemySpawner spawner;
    private int lastWaveNumber = 0;

    // ──────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

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
        EnemyEvents.OnEnemyDeathWithDistance += HandleEnemyDeathFeedback;
    }

    private void OnDisable()
    {
        WaveChecker.On10WavesCompleted -= Handle10WavesCompleted;
        EnemyEvents.OnEnemyDeathWithDistance -= HandleEnemyDeathFeedback;
    }

    // ──────────────────────────────
    private void Handle10WavesCompleted(int waveNumber)
    {
        int waveDelta = waveNumber - lastWaveNumber;
        accumulatedHealth *= 1 + (waveDelta * waveGrowthRate);
        accumulatedSpeed *= 1 + (waveDelta * waveGrowthRate / 2);
        accumulatedDamage *= 1 + (waveDelta * waveGrowthRate);
        accumulatedSpawnRate *= 1 + (waveDelta * waveGrowthRate / 3);

        lastWaveNumber = waveNumber;
        ApplyState();
    }

    private void HandleEnemyDeathFeedback(float travelPercent)
    {
        // travelPercent = 0 (dies early) → too easy
        // travelPercent = 1 (reaches end) → too hard

        if (travelPercent < 0.3f)
        {
            accumulatedHealth *= 1 + distanceHardFactor;
            accumulatedSpeed *= 1 + distanceHardFactor;
        }
        else if (travelPercent > 0.8f)
        {
            accumulatedHealth *= 1 - distanceEaseFactor;
            accumulatedSpeed *= 1 - distanceEaseFactor;
        }

        ClampMultipliers();
        ApplyState();
    }

    // ──────────────────────────────
    private void ApplyState()
    {
        ClampMultipliers();

        currentState.healthMultiplier = accumulatedHealth;
        currentState.speedMultiplier = accumulatedSpeed;
        currentState.damageMultiplier = accumulatedDamage;
        currentState.spawnRateMultiplier = accumulatedSpawnRate;

        OnDifficultyChanged?.Invoke(currentState);

        if (spawner != null)
            spawner.SetSpawnTimeMultiplier(currentState.spawnRateMultiplier);

        Debug.Log($"[WaveDifficultyManager] Difficulty updated → " +
                  $"Health:{currentState.healthMultiplier:F2}, " +
                  $"Speed:{currentState.speedMultiplier:F2}, " +
                  $"Damage:{currentState.damageMultiplier:F2}, " +
                  $"Spawn:{currentState.spawnRateMultiplier:F2}");
    }

    private void ClampMultipliers()
    {
        accumulatedHealth = Mathf.Clamp(accumulatedHealth, minMultiplier, maxMultiplier);
        accumulatedSpeed = Mathf.Clamp(accumulatedSpeed, minMultiplier, maxMultiplier);
        accumulatedDamage = Mathf.Clamp(accumulatedDamage, minMultiplier, maxMultiplier);
        accumulatedSpawnRate = Mathf.Clamp(accumulatedSpawnRate, minMultiplier, maxMultiplier);
    }

    public DifficultyState GetCurrentState() => currentState;
}
