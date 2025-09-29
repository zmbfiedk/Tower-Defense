using UnityEngine;
using UnityEngine.EventSystems;

public class Dragscript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Tower Settings")]
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private float snapDistance = 1f;
    [SerializeField] private string towerPlacementTag = "TowerPlacement";
    [SerializeField] private int towerCost = 50;

    public int cost
    {
        get { return towerCost; }
    }

    private GameObject towerClone;

    // -------------------------
    // Drag Events
    // -------------------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!towerPrefab) return;

        towerClone = Instantiate(towerPrefab, GetMouseWorldPos(eventData), Quaternion.identity);
        SetPreviewMode(towerClone, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (towerClone)
            towerClone.transform.position = GetMouseWorldPos(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!towerClone) return;

        TowerSpot spot = FindClosestSpot(towerClone.transform.position);

        if (spot != null && !spot.HasTower && CurrencyManager.Instance.SpendCurrency(towerCost))
        {
            PlaceTower(towerClone, spot);
        }
        else
        {
            Destroy(towerClone);
            Debug.Log("Tower placement failed (invalid spot or not enough currency).");
        }

        towerClone = null;
    }

    // -------------------------
    // Placement Helpers
    // -------------------------
    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        return worldPos;
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

        SetPreviewMode(tower, false);

        Rigidbody2D rb = tower.GetComponent<Rigidbody2D>() ?? tower.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        Tower towerComp = tower.GetComponent<Tower>() ?? tower.AddComponent<Tower>();
        towerComp.SetSpot(spot);

        spot.PlaceTower(tower);
    }

    private void SetPreviewMode(GameObject tower, bool isPreview)
    {
        foreach (var r in tower.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = r.color;
            c.a = isPreview ? 0.5f : 1f;
            r.color = c;
        }

        foreach (var col in tower.GetComponentsInChildren<Collider2D>())
            col.enabled = !isPreview;
    }
}
