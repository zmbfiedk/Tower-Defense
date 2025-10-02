using UnityEngine;

public static class TowerFactory
{
    public static GameObject CreatePreview(GameObject prefab, Vector3 position)
    {
        GameObject preview = Object.Instantiate(prefab, position, Quaternion.identity);
        TowerPreview.SetPreviewMode(preview, true);
        return preview;
    }

    public static void FinalizeTower(GameObject tower)
    {
        TowerPreview.SetPreviewMode(tower, false);

        Rigidbody2D rb = tower.GetComponent<Rigidbody2D>() ?? tower.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        tower.AddComponent<SellTower>();
    }
}
