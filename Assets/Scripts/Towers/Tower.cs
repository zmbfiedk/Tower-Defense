using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour
{
    private TowerSpot spot;
    private float lastClickTime;
    private const float doubleClickDelay = 0.3f;
    Dragscript Dragscript => FindObjectOfType<Dragscript>();
    public void SetSpot(TowerSpot s) => spot = s;
    private int towerPrice;

    private void Start()
    {
        towerPrice = Dragscript.cost;
    }

    private void OnMouseDown()
    {
        if (Time.time - lastClickTime <= doubleClickDelay)
            RemoveTower();

        lastClickTime = Time.time;
    }

    private void RemoveTower()
    {
        if (spot != null)
            spot.RemoveTower();
        else
            Destroy(gameObject);
        CurrencyManager.Instance.AddCurrency(towerPrice/4);
    }
}
