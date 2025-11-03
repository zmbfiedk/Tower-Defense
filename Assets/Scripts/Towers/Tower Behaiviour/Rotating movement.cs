using UnityEngine;

public class RotatingMovement: MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float orbitRadius = 3f;

    private Transform spot;
    private float angle = 90f;
    public bool IsPlaced { get; private set; } = false;

    private void Update()
    {
        if (!IsPlaced)
        {
            if (transform.parent != null)
            {
                spot = transform.parent;
                IsPlaced = true;
                transform.position = spot.position + new Vector3(0f, orbitRadius, 0f);
            }
            else return;
        }

        Orbit();
    }

    private void Orbit()
    {
        angle += rotationSpeed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        float rad = angle * Mathf.Deg2Rad;
        float x = spot.position.x + orbitRadius * Mathf.Cos(rad);
        float y = spot.position.y + orbitRadius * Mathf.Sin(rad);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public Vector3 GetSpotPosition() => spot != null ? spot.position : transform.position;
    public float GetRadius() => orbitRadius;

    private void OnDrawGizmosSelected()
    {
        if (spot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spot.position, orbitRadius);
        }
    }
}
