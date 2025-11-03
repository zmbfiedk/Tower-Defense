using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerPreview : MonoBehaviour
{

    [Header("Preview Settings")]
    [SerializeField] private float radius = 3f; // Set manually per tower prefab
    [SerializeField] private int segments = 50; // Smoothness of circle

    private SpriteRenderer sr;
    private LineRenderer lr;
    private Color originalColor;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (!sr)
            sr = gameObject.AddComponent<SpriteRenderer>();

        originalColor = sr.color;

        foreach (var comp in GetComponents<MonoBehaviour>())
        {
            if (!(comp is TowerPreview))
                comp.enabled = false;
        }

        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        lr = gameObject.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.loop = true;
        lr.widthMultiplier = 0.05f;
        lr.positionCount = segments;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.sortingOrder = 10;

        lr.enabled = true; // Initially hidden
        DrawCircle();
    }

    private void DrawCircle()
    {
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            lr.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    public void SetColor(Color c)
    {
        if (sr) sr.color = c;
        if (lr)
        {
            lr.startColor = c;
            lr.endColor = c;
        }
    }


    private void HandlePlaced(TowerPreview placedPreview)
    {
        if (placedPreview == this)
        {
            if (lr) lr.enabled = true;
            if (sr) sr.color = originalColor;
        }
    }


    public void OnPlaced()
    {
        if (lr) lr.enabled = false;

        if (sr) sr.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    }

}
