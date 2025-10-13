using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject towerPrefab;

    private GameObject previewTower;
    private TowerPreview previewScript;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!towerPrefab) return;

        // Create preview tower
        previewTower = TowerFactory.CreatePreview(towerPrefab, GetMouseWorldPos(eventData));
        previewScript = previewTower.GetComponent<TowerPreview>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!previewTower) return;

        // Move preview with mouse
        Vector3 worldPos = GetMouseWorldPos(eventData);
        previewTower.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);

        // Check valid placement
        TowerSpot spot = TowerPlacementManager.Instance.GetClosestSpot(previewTower.transform.position);
        bool canPlace = (spot != null && !spot.HasTower);

        // Change preview color
        if (previewScript != null)
        {
            if (canPlace)
                previewScript.SetColor(Color.green);
            else
                previewScript.SetColor(Color.red);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!previewTower) return;

        TowerPlacementManager.Instance.TryPlaceTower(previewTower);
        previewTower = null;
        previewScript = null;
    }

    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f; // Ensure preview is on the correct layer
        return worldPos;
    }
}
