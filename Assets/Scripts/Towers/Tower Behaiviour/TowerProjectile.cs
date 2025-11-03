using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField]private float damage = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
           if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
