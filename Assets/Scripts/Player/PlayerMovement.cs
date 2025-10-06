using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    private Rigidbody2D rb;


    // Movement variables
    private float currentSpeed;
    private Vector2 moveDirection;
    [SerializeField] private float moveSpeed = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;
        rb.velocity = moveDirection * currentSpeed;
    }



}