using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f); // adjust in inspector
    private Transform target;

    private void Awake()
    {
        if (fillImage == null)
            fillImage = GetComponentInChildren<Image>();
    }

    public void Initialize(Transform enemyTransform)
    {
        target = enemyTransform;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    public void UpdateHealth(float current, float max)
    {
        if (fillImage == null) return;
        fillImage.fillAmount = current / max;
    }
}
