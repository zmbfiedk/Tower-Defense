using UnityEngine;

public static class TowerFactory
{
    public static GameObject CreatePreview(GameObject prefab, Vector3 position)
    {
        GameObject preview = Object.Instantiate(prefab, position, Quaternion.identity);

        foreach (var c in preview.GetComponents<Collider2D>())
            c.enabled = false;

        foreach (var comp in preview.GetComponents<MonoBehaviour>())
        {
            if (!(comp is TowerPreview))
                comp.enabled = false;
        }

        if (!preview.GetComponent<TowerPreview>())
            preview.AddComponent<TowerPreview>();

        return preview;
    }

    public static void FinalizeTower(GameObject tower)
    {
        TowerPreview preview = tower.GetComponent<TowerPreview>();
        if (preview) Object.Destroy(preview);

        foreach (var comp in tower.GetComponents<MonoBehaviour>())
            comp.enabled = true;

        foreach (var col in tower.GetComponents<Collider2D>())
            col.enabled = true;
    }

}
