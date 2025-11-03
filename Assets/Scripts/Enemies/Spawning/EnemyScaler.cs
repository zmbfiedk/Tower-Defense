using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyScaler : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private EnemyPath enemyPath;

    private float baseMaxHealth;
    private float baseSpeed;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyPath = GetComponent<EnemyPath>();

        baseMaxHealth = enemyHealth != null ? enemyHealth.GetBaseMaxHealth() : 1f;
        baseSpeed = enemyPath != null ? enemyPath.Speed : 1f;
    }

    private void OnEnable()
    {
        WaveDifficultyManager.OnDifficultyChanged += OnDifficultyChanged;
    }

    private void OnDisable()
    {
        WaveDifficultyManager.OnDifficultyChanged -= OnDifficultyChanged;
    }

    private void Start()
    {
        if (WaveDifficultyManager.Instance != null)
        {
            var state = WaveDifficultyManager.Instance.GetCurrentState();
            ApplyState(state);
        }
    }

    private void OnDifficultyChanged(WaveDifficultyManager.DifficultyState state)
    {
        ApplyState(state);
    }

    private void ApplyState(WaveDifficultyManager.DifficultyState state)
    {
        if (enemyHealth != null)
            enemyHealth.ApplyHealthMultiplier(state.healthMultiplier);

        if (enemyPath != null)
            enemyPath.ApplySpeedMultiplier(state.speedMultiplier);
    }
}
