using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SellTower : MonoBehaviour
{
    private TowerSpot spot;
    private float lastClickTime;
    private const float doubleClickDelay = 0.3f;

    [Header("Tower Settings")]
    [SerializeField] private int towerPrice = 50; // Already set in prefab

    public int TowerPrice => towerPrice;

    private Camera mainCam;
    private Collider2D parentCollider;

    private void Awake()
    {
        mainCam = Camera.main;
        parentCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Get mouse position in world
        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Check all colliders under the mouse
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld);

        if (hits == null || hits.Length == 0) return;

        // Only proceed if the parent collider was clicked
        foreach (var hit in hits)
        {
            if (hit == parentCollider)
            {
                // Double click check
                if (Time.time - lastClickTime <= doubleClickDelay)
                    Sell();

                lastClickTime = Time.time;
                break;
            }
        }
    }

    private void Sell()
    {
        if (spot != null)
        {
            spot.RemoveTower();
            spot = null;
        }

        // Destroy all children (tower visuals)
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        // Refund half the tower's price
        CurrencyManager.Instance.AddCurrency(towerPrice / 2);

        Debug.Log($"{name} sold for {towerPrice / 2}");
    }

    // Assign spot when placing tower
    public void SetSpot(TowerSpot s) => spot = s;
}
