using UnityEngine;

public static class TowerPreview
{
    public static void SetPreviewMode(GameObject tower, bool isPreview)
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
