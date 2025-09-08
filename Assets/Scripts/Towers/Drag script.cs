using UnityEngine;
using UnityEngine.EventSystems;

public class Dragscript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Tower Settings")]
    [SerializeField] private GameObject towerPrefab;  
    [SerializeField] private float snapDistance = 1f;  
    [SerializeField] private string towerPlacementTag = "TowerPlacement";

    private GameObject towerClone; 

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (towerPrefab == null) return;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        towerClone = Instantiate(towerPrefab, worldPos, Quaternion.identity);
        SetPreviewMode(towerClone, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (towerClone == null) return;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        towerClone.transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (towerClone == null) return;

        GameObject[] placements = GameObject.FindGameObjectsWithTag(towerPlacementTag);
        float closestDistance = Mathf.Infinity;
        Transform closestPlacement = null;

        foreach (GameObject placement in placements)
        {
            float distance = Vector3.Distance(towerClone.transform.position, placement.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlacement = placement.transform;
            }
        }

        if (closestPlacement != null && closestDistance <= snapDistance)
        {
            TowerSpot spot = closestPlacement.GetComponent<TowerSpot>();
            if (spot != null)
            {
                if (!spot.HasTower)
                {
                    towerClone.transform.position = closestPlacement.position;
                    SetPreviewMode(towerClone, false);
                    PreparePlacedTower(towerClone, spot);
                    spot.PlaceTower(towerClone); 
                }
                else
                {
                    Destroy(towerClone);
                }
            }
            else
            {
                Destroy(towerClone);
            }
        }
        else
        {
            Destroy(towerClone);
        }

        towerClone = null;
    }

    private void PreparePlacedTower(GameObject tower, TowerSpot spot)
    {
        tower.tag = "Tower";

        Collider2D[] cols = tower.GetComponentsInChildren<Collider2D>();
        foreach (var c in cols) c.enabled = true;

        Rigidbody2D rb = tower.GetComponent<Rigidbody2D>();
        if (rb == null) rb = tower.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        Tower towerComp = tower.GetComponent<Tower>();
        if (towerComp == null) towerComp = tower.AddComponent<Tower>();
        towerComp.SetSpot(spot);

        tower.transform.SetParent(spot.transform, worldPositionStays: true);
    }

    private void SetPreviewMode(GameObject tower, bool isPreview)
    {
        SpriteRenderer[] renderers = tower.GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            Color c = r.color;
            c.a = isPreview ? 0.5f : 1f;
            r.color = c;
        }

        Collider2D[] colliders = tower.GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders) col.enabled = !isPreview;
    }
}
