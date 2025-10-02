using UnityEngine;

[RequireComponent(typeof(EnemyPath))]
public class EndPointDamage : MonoBehaviour
{
    [SerializeField] private int damage = 15;

    private void OnEnable()
    {
        GetComponent<EnemyPath>().OnReachedEnd += DamagePlayer;
    }

    private void OnDisable()
    {
        GetComponent<EnemyPath>().OnReachedEnd -= DamagePlayer;
    }

    private void DamagePlayer()
    {
        PlayerHealth.instance.TakeDamage(damage);
        Destroy(gameObject);
    }
}
