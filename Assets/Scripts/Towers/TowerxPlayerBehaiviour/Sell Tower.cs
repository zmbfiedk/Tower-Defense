using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SellTower : MonoBehaviour
{
    private TowerSpot spot;
    private float lastClickTime;
    private const float doubleClickDelay = 0.3f;
    [SerializeField] private int towerPrice = 50; // default if not set

    public void SetSpot(TowerSpot s) => spot = s;

    public void SetTowerPrice(int price)
    {
        towerPrice = price;
    }

    private void OnMouseDown()
    {
        if (Time.time - lastClickTime <= doubleClickDelay)
        {
            Sell();
        }
        lastClickTime = Time.time;
    }

    private void Sell()
    {
        if (spot != null)
            spot.RemoveTower();
        else
            Destroy(gameObject);

        CurrencyManager.Instance.AddCurrency(towerPrice / 4);
    }
}
