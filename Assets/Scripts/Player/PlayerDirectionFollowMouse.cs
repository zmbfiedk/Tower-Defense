using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionFollowMouse : MonoBehaviour
{
    void Update()
    {
        RotatePlayerToMouse();
    }

    private void RotatePlayerToMouse()
    {
        if (Camera.main == null) return;

        Vector2 mouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
