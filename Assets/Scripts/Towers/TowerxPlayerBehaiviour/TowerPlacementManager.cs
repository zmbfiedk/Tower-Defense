using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance;

    [SerializeField] private string towerPlacementTag = "TowerPlacement";
    [SerializeField] private float snapDistance = 1f;
    [SerializeField] private int towerCost = 50;

    private void Awake() => Instance = this;

    public void TryPlaceTower(GameObject tower)
    {
        TowerSpot spot = FindClosestSpot(tower.transform.position);

        if (spot != null && !spot.HasTower && CurrencyManager.Instance.SpendCurrency(towerCost))
        {
            PlaceTower(tower, spot);
        }
        else
        {
            Object.Destroy(tower);
            Debug.Log("Tower placement failed (invalid spot or not enough currency).");
        }
    }

    private TowerSpot FindClosestSpot(Vector3 position)
    {
        GameObject[] placements = GameObject.FindGameObjectsWithTag(towerPlacementTag);
        TowerSpot closestSpot = null;
        float closestDist = snapDistance;

        foreach (GameObject placement in placements)
        {
            float dist = Vector3.Distance(position, placement.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestSpot = placement.GetComponent<TowerSpot>();
            }
        }

        return closestSpot;
    }

    private void PlaceTower(GameObject tower, TowerSpot spot)
    {
        tower.transform.position = spot.transform.position;
        tower.transform.SetParent(spot.transform);
        TowerFactory.FinalizeTower(tower);

        spot.PlaceTower(tower);
    }
}
