using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject towerPrefab;

    private GameObject previewTower;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!towerPrefab) return;

        previewTower = TowerFactory.CreatePreview(towerPrefab, GetMouseWorldPos(eventData));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (previewTower)
            previewTower.transform.position = GetMouseWorldPos(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!previewTower) return;

        TowerPlacementManager.Instance.TryPlaceTower(previewTower);
        previewTower = null;
    }

    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        return worldPos;
    }
}
