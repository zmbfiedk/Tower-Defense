using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tower : MonoBehaviour
{
    private TowerSpot spot;
    private float lastClickTime;
    private const float doubleClickDelay = 0.3f;

    public void SetSpot(TowerSpot s) => spot = s;

    private void OnMouseDown()
    {
        if (Time.time - lastClickTime <= doubleClickDelay)
            RemoveTower();

        lastClickTime = Time.time;
    }

    private void RemoveTower()
    {
        if (spot != null)
            spot.RemoveTower();
        else
            Destroy(gameObject);
    }
}
