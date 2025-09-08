using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static event Action OnEnemySpawn;

    [Header("Spawn Timing")]
    [SerializeField] private float minimumSpawnTime = 1f;
    [SerializeField] private float maximumSpawnTime = 5f;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs = new GameObject[6]; // 6 enemy types

    private float timeTillSpawn;
    private WaveChecker waveChecker;
    [SerializeField] private bool canSpawn = true;

    private void Awake()
    {
        // Ensure min and max spawn times are sensible
        if (minimumSpawnTime > maximumSpawnTime)
        {
            float temp = minimumSpawnTime;
            minimumSpawnTime = maximumSpawnTime;
            maximumSpawnTime = temp;
        }

        waveChecker = FindObjectOfType<WaveChecker>();
        SetTimeUntilSpawn();
    }

    private void Start()
    {
        if (waveChecker != null)
        {
            WaveChecker.OnMaxEnemySpawn += StopSpawning;
            WaveChecker.OnWaveOver += StopSpawning;
            WaveChecker.OnWaveOver += () => Invoke(nameof(AllowSpawning), 9f);
        }
        else
        {
            Debug.LogError("WaveChecker not found in scene!");
        }
    }

    private void Update()
    {
        if (waveChecker == null || !waveChecker.IsWaveActive()) return;

        timeTillSpawn -= Time.deltaTime;

        if (timeTillSpawn <= 0 && canSpawn)
        {
            // Use public getter method from WaveChecker
            if (waveChecker.GetCurrentEnemiesInScene() >= waveChecker.GetEnemiesToKillThisWave())
            {
                canSpawn = false;
                return;
            }
            

            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeTillSpawn = UnityEngine.Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        int enemyTypeIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[enemyTypeIndex];

        if (enemyToSpawn != null)
        {
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            Debug.Log($"Spawned {enemyToSpawn.name} at level {waveChecker.GetLevel()} from '{transform.name}'");

            Pathing pathing = spawnedEnemy.GetComponent<Pathing>();
            if (pathing != null)
                pathing.OnReachedEnd += waveChecker.EnemyReachedEnd;

            OnEnemySpawn?.Invoke();
        }
    }


    private void StopSpawning()
    {
        canSpawn = false;
    }

    private void AllowSpawning()
    {
        canSpawn = true;
    }
}
