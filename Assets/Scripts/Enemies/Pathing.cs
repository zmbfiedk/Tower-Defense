using System;
using UnityEngine;

public class Pathing : MonoBehaviour
{
    public event Action OnReachedEnd;

    [Header("Tags")]
    [SerializeField] private string spawnPointTag = "SpawnPoint";
    [SerializeField] private string patrolPointTag = "PatrolPoint";
    [SerializeField] private string endPointTag = "EndPoint";

    [Header("Settings")]
    [SerializeField] private float speed = 1f;

    private Transform[] patrolPoints;
    private int currentTargetIndex = 0;

    private void Start()
    {
        // Get spawn
        GameObject spawnObj = GameObject.FindWithTag(spawnPointTag);
        if (spawnObj != null) transform.position = spawnObj.transform.position;

        // Get patrol points (ordered by name)
        GameObject[] points = GameObject.FindGameObjectsWithTag(patrolPointTag);
        Array.Sort(points, (a, b) => a.name.CompareTo(b.name));

        // Get endpoint
        GameObject endObj = GameObject.FindWithTag(endPointTag);

        // Build path
        int totalPoints = points.Length + (endObj != null ? 1 : 0);
        patrolPoints = new Transform[totalPoints];
        for (int i = 0; i < points.Length; i++)
            patrolPoints[i] = points[i].transform;
        if (endObj != null)
            patrolPoints[totalPoints - 1] = endObj.transform;
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

            // Enemy reached the end
            if (currentTargetIndex >= patrolPoints.Length)
            {
                OnReachedEnd?.Invoke();
                Destroy(gameObject); 
            }
        }
    }
}
