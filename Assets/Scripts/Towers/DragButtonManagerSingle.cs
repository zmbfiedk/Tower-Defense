using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// Serializable data container for UI buttons and tower prefabs
[System.Serializable]
public class TowerData
{
    public Button button;          // UI Button for this tower
    public GameObject towerPrefab; // Corresponding tower prefab
}

public class DragButtonManagerSingle : MonoBehaviour
{
    [Header("Towers Setup")]
    public TowerData[] towers; 
    public float snapDistance = 1f; 

    private GameObject towerClone;          
    private GameObject draggingTowerPrefab;  

    private void Start()
    {
        foreach (var tower in towers)
        {
            tower.button.onClick.AddListener(() => StartDrag(tower.towerPrefab));
        }
    }

    private void Update()
    {
        if (towerClone != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            towerClone.transform.position = mousePos;
        }

        if (towerClone != null && Input.GetMouseButtonUp(0))
        {
            PlaceOrDestroyTower();
        }
    }

    private void StartDrag(GameObject prefab)
    {
        draggingTowerPrefab = prefab;
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos.z = 0;
        towerClone = Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    private void PlaceOrDestroyTower()
    {
        GameObject[] placements = GameObject.FindGameObjectsWithTag("TowerPlacement");
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
            towerClone.transform.position = closestPlacement.position;
        }
        else
        {
            Destroy(towerClone);
        }

        towerClone = null;
        draggingTowerPrefab = null;
    }
}
