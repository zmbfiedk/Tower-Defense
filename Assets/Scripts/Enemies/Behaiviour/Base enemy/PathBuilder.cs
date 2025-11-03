using System;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private string spawnPointTag = "SpawnPoint";
    [SerializeField] private string patrolPointTag = "PatrolPoint";
    [SerializeField] private string endPointTag = "EndPoint";

    public Transform[] BuildPath()
    {
        GameObject spawnObj = GameObject.FindWithTag(spawnPointTag);
        GameObject[] points = GameObject.FindGameObjectsWithTag(patrolPointTag);
        Array.Sort(points, (a, b) => a.name.CompareTo(b.name));
        GameObject endObj = GameObject.FindWithTag(endPointTag);

        int totalPoints = points.Length + 2; // spawn + patrol + end
        Transform[] allPoints = new Transform[totalPoints];
        int index = 0;

        if (spawnObj != null)
            allPoints[index++] = spawnObj.transform;

        foreach (var p in points)
            allPoints[index++] = p.transform;

        if (endObj != null)
            allPoints[index] = endObj.transform;

        return allPoints;
    }
}
