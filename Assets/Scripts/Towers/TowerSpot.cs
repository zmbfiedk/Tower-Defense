using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private GameObject currentTower;

    public bool HasTower => currentTower != null;
    public bool PlaceTower(GameObject tower)
    {
        if (HasTower) return false;
        currentTower = tower;
   
        currentTower.transform.position = transform.position;
        currentTower.transform.SetParent(transform, worldPositionStays: true);

        var t = currentTower.GetComponent<Tower>();
        if (t != null) t.SetSpot(this);

        return true;
    }

    public void RemoveTower()
    {
        if (!HasTower) return;
        Destroy(currentTower);
        currentTower = null;
    }
}
