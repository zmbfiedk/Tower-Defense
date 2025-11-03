using UnityEngine;

namespace _2DCameraAim
{
    public class BasicPlayerMovement : MonoBehaviour
    {
        private Rigidbody2D rb;

        public float moveSpeed = 2;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 movementDirection;
            movementDirection.x = Input.GetAxisRaw("Horizontal");
            movementDirection.y = Input.GetAxisRaw("Vertical");
            movementDirection = movementDirection.normalized;

            rb.velocity = movementDirection * moveSpeed;
        }
    }
}
