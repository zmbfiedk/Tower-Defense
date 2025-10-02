using System;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public event Action OnReachedEnd;

    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform[] patrolPoints;
    private int currentTargetIndex = 0;

    public float Speed => speed;



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
}
