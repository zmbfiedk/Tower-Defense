using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyvariable : MonoBehaviour
{
    [SerializeField] Pathing EnemyVariables;
    [SerializeField] float Speed = 1;
    [SerializeField] int Damage = 1;
    void Start()
    {
        EnemyVariables.Speed = Speed;
        EnemyVariables.Damage = Damage;
    }

    void Update()
    {
        
    }
}
