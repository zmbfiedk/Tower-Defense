using System;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public event Action OnReachedEnd;

    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform[] patrolPoints;
    private int currentTargetIndex = 0;

    public float Speed => speed;

    private float baseSpeed;

    private void Awake()
    {
        baseSpeed = speed;
    }

    public void Initialize(Transform[] points)
    {
        patrolPoints = points;
        transform.position = patrolPoints[0].position;
    }

    private void Update()
    {
        if (patrolPoints == null || currentTargetIndex >= patrolPoints.Length) return;

        Transform target = patrolPoints[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            transform.position = target.position;
            currentTargetIndex++;

            if (currentTargetIndex >= patrolPoints.Length)
                OnReachedEnd?.Invoke();
        }
    }

    public void ApplySpeedMultiplier(float multiplier)
    {
        if (multiplier <= 0f) multiplier = 1f;
        speed = baseSpeed * multiplier;
    }

    public float GetTravelProgress()
    {
        if (patrolPoints == null || patrolPoints.Length < 2) return 0f;

        float totalDistance = 0f;
        for (int i = 0; i < patrolPoints.Length - 1; i++)
            totalDistance += Vector3.Distance(patrolPoints[i].position, patrolPoints[i + 1].position);

        float traveledDistance = 0f;
        for (int i = 1; i < currentTargetIndex && i < patrolPoints.Length; i++)
            traveledDistance += Vector3.Distance(patrolPoints[i - 1].position, patrolPoints[i].position);

        // add partial distance to next point
        if (currentTargetIndex < patrolPoints.Length)
            traveledDistance += Vector3.Distance(transform.position, patrolPoints[currentTargetIndex].position) / 2f;

        return Mathf.Clamp01(traveledDistance / totalDistance);
    }

}
