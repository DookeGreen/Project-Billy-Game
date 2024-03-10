using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Contoller : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 mousePos;
    private Vector2 movement;
    private Vector3 lastPosition;
    private Vector3 movementDirection;
    private int count;

    void Update()
    {
        if(Input.GetKeyDown("b"))
        {
            Debug.Log("Dash");
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        anim.SetFloat("Horizontal", mousePos.x);
        anim.SetFloat("Vertical", mousePos.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
        movementDirection = new Vector3(movement.x, movement.y, 0f).normalized;
    }
    void FixedUpdate()
    {
        rb.velocity = (movement.normalized * movementSpeed);
    }
}