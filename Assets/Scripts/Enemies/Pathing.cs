using UnityEngine;

public class Pathing : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string spawnPointTag = "SpawnPoint";
    [SerializeField] private string patrolPointTag = "PatrolPoint";
    [SerializeField] private string endPointTag = "EndPoint";

    [Header("Settings")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;

    private Transform[] patrolPoints;
    private Transform spawnPoint;
    private Transform endPoint;
    private int currentTargetIndex = 0;
    private bool reachedEnd = false;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    private void Start()
    {
        GameObject spawnObj = GameObject.FindWithTag(spawnPointTag);
        if (spawnObj != null)
        {
            spawnPoint = spawnObj.transform;
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning($"No object found with tag '{spawnPointTag}'! Enemy will stay at current position.");
        }

        GameObject[] points = GameObject.FindGameObjectsWithTag(patrolPointTag);
        System.Array.Sort(points, (a, b) => a.name.CompareTo(b.name));

        GameObject endObj = GameObject.FindWithTag(endPointTag);
        if (endObj != null)
            endPoint = endObj.transform;
        else
            Debug.LogWarning($"No object found with tag '{endPointTag}'! Enemy will stop at last patrol point.");

        int totalPoints = points.Length + (endPoint != null ? 1 : 0);
        patrolPoints = new Transform[totalPoints];
        for (int i = 0; i < points.Length; i++)
            patrolPoints[i] = points[i].transform;
        if (endPoint != null)
            patrolPoints[totalPoints - 1] = endPoint;
    }

    private void Update()
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || reachedEnd) return;

        Transform targetPoint = patrolPoints[currentTargetIndex];
        Vector3 moveDirection = (targetPoint.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.position += moveDirection * distanceThisFrame;

        // Check if reached target
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            transform.position = targetPoint.position;

            if (currentTargetIndex == patrolPoints.Length - 1)
            {
                reachedEnd = true;
                DieAtEnd();
            }
            else
            {
                currentTargetIndex++;
            }
        }
    }

    private void DieAtEnd()
    {

        Destroy(gameObject);
    }
}
