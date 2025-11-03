using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [SerializeField] private int reward = 5;

    public void GiveReward()
    {
        CurrencyManager.Instance?.AddCurrency(reward);
    }
}
