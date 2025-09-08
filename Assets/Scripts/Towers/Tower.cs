using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour
{
    private TowerSpot spot;
    private float lastClickTime = 0f;
    private float doubleClickDelay = 0.3f;

    public void SetSpot(TowerSpot s)
    {
        spot = s;
    }

    private void OnMouseDown()
    {
        if (Time.time - lastClickTime <= doubleClickDelay)
        {
            if (spot != null) spot.RemoveTower();
            else Destroy(gameObject);
        }
        lastClickTime = Time.time;
    }
}
