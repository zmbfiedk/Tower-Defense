using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 10f;
    public float speed = 2f;
    public int reward = 5; // currency given when killed

    private float currentHealth;

    [Header("Path Settings")]
    public Transform[] pathPoints; 
    private int currentPointIndex = 0;

    void Start()
    {
        currentHealth = maxHealth;

        if (pathPoints.Length > 0)
        {
            transform.position = pathPoints[0].position;
            currentPointIndex = 1;
        }
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (currentPointIndex >= pathPoints.Length) return;

        Vector3 target = pathPoints[currentPointIndex].position;
        Vector3 direction = (target - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex++;

            if (currentPointIndex >= pathPoints.Length)
            {
                ReachGoal();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        CurrencyManager.Instance.AddCurrency(reward); 

        Destroy(gameObject);
    }

    void ReachGoal()
    {
        Destroy(gameObject);
    }
}
