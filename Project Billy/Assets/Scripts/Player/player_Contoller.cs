using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Contoller : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Animator anim;
    private Vector2 mousePos;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        anim.SetFloat("Horizontal", mousePos.x);
        anim.SetFloat("Vertical", mousePos.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.velocity = (movement.normalized * movementSpeed);
    }
}
