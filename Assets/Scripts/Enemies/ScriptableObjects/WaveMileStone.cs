using UnityEngine;

[CreateAssetMenu(fileName = "WaveMilestone", menuName = "Wave/Wave Milestone", order = 0)]
public class WaveMilestone : ScriptableObject
{
    [Header("Trigger")]
    public int waveNumber = 10; // e.g. 10, 20, 30...

    [Header("Multipliers (multiplicative, stacked)")]
    public float healthMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float damageMultiplier = 1f;

    [Header("Spawner / Gameplay toggles")]
    public float spawnRateMultiplier = 1f; 
    public bool unlockHealer = false;
    public bool unlockInvisible = false;
    public bool unlockNewEnemy = false;
    public GameObject newEnemyPrefab = null; 
}
