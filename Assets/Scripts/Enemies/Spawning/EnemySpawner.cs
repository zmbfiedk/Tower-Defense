using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static event Action OnEnemySpawn;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 5f;

    [Header("Enemy Prefabs")]
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private GameObject invisibleEnemyPrefab;
    [SerializeField] private GameObject healerEnemyPrefab;

    private float timeTillSpawn;
    private bool canSpawn = true;
    private WaveChecker waveChecker;
    private bool healerUnlocked = false;
    private bool invisibleUnlocked = false;
    private float spawnTimeMultiplier = 1f; 
    private void Awake()
    {
        if (minSpawnTime > maxSpawnTime)
            (minSpawnTime, maxSpawnTime) = (maxSpawnTime, minSpawnTime);

        waveChecker = FindObjectOfType<WaveChecker>();
        ResetSpawnTimer();
    }

    private void Start()
    {
        if (waveChecker != null)
        {
            WaveChecker.OnMaxEnemySpawn += StopSpawning;
            WaveChecker.OnWaveOver += StopSpawning;
            WaveChecker.OnWaveOver += () => Invoke(nameof(AllowSpawning), 9f);

            WaveChecker.OnBossWave += () =>
            {
                StopSpawning();
                SpawnBoss();
            };

            EnemyEvents.OnBossDefeated += () =>
            {
                Debug.Log("[EnemySpawner] Boss defeated, resuming normal spawns.");
                AllowSpawning();
            };
        }
    }

    private void Update()
    {
        if (waveChecker == null || !waveChecker.IsWaveActive() || !canSpawn) return;

        timeTillSpawn -= Time.deltaTime;
        if (timeTillSpawn <= 0)
        {
            if (waveChecker.GetCurrentEnemiesInScene() < waveChecker.GetEnemiesToKillThisWave())
                SpawnEnemy();

            ResetSpawnTimer();
        }
    }

    private void ResetSpawnTimer()
    {
        float effectiveMin = minSpawnTime * spawnTimeMultiplier;
        float effectiveMax = maxSpawnTime * spawnTimeMultiplier;
        timeTillSpawn = UnityEngine.Random.Range(effectiveMin, effectiveMax);
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0) return;

        int currentWave = waveChecker.GetCurrentWaveNumber();

        GameObject prefabToSpawn = null;

        if (currentWave >= 20 && invisibleUnlocked && invisibleEnemyPrefab != null && UnityEngine.Random.value < 0.2f)
        {
            prefabToSpawn = invisibleEnemyPrefab;
        }
        else if (currentWave >= 10 && healerUnlocked && healerEnemyPrefab != null && UnityEngine.Random.value < 0.3f)
        {
            prefabToSpawn = healerEnemyPrefab;
        }
        else
        {
            prefabToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
        }

        if (prefabToSpawn == null) return;

        GameObject enemyObj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        // --- Initialize path ---
        PathBuilder pathBuilder = FindObjectOfType<PathBuilder>();
        if (pathBuilder != null)
        {
            EnemyPath pathComp = enemyObj.GetComponent<EnemyPath>();
            if (pathComp != null)
                pathComp.Initialize(pathBuilder.BuildPath());
        }

        OnEnemySpawn?.Invoke();
    }

    private void SpawnBoss()
    {
        if (bossPrefabs.Length == 0) return;

        GameObject prefab = bossPrefabs[UnityEngine.Random.Range(0, bossPrefabs.Length)];
        GameObject enemyObj = Instantiate(prefab, transform.position, Quaternion.identity);
        PathBuilder pathBuilder = FindObjectOfType<PathBuilder>();
        if (pathBuilder != null)
        {
            EnemyPath pathComp = enemyObj.GetComponent<EnemyPath>();
            if (pathComp != null)
                pathComp.Initialize(pathBuilder.BuildPath());
        }

        OnEnemySpawn?.Invoke();
    }

    private void StopSpawning() => canSpawn = false;
    private void AllowSpawning() => canSpawn = true;
    public void SetSpawnTimeMultiplier(float multiplier)
    {
        spawnTimeMultiplier = Mathf.Max(0.01f, multiplier);
        ResetSpawnTimer();
    }

    public void UnlockHealer() => healerUnlocked = true;
    public void UnlockInvisible() => invisibleUnlocked = true;

    public void AddEnemyPrefabToPool(GameObject prefab)
    {
        if (prefab == null) return;
        if (!enemyPrefabs.Contains(prefab))
            enemyPrefabs.Add(prefab);
    }
}
