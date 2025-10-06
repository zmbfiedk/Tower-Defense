using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static event Action OnEnemySpawn;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 5f;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;

    private float timeTillSpawn;
    private bool canSpawn = true;
    private WaveChecker waveChecker;

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
        timeTillSpawn = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject prefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyObj = Instantiate(prefab, transform.position, Quaternion.identity);

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
}
