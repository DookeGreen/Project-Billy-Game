using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Contoller : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [Range(0.1f, 10f)]
    [SerializeField] private float dodgeForce;
    [SerializeField] private string enemyLayerName;
    [SerializeField] private GameObject dodgeOBJ;
    [Range(50f, 250f)]
    [SerializeField] private float maxStamina;
    [Range(0f, 250f)]
    [SerializeField] private float dodgeCost;
    [Range(0f, 20f)]
    [SerializeField] private float dodgeRegen;
    private float currentStamina;
    private Vector2 mousePos;
    private Vector2 movement;
    private Vector3 lastPosition;
    private Vector3 movementDirection;
    private int count;
    public bool dodging;
    private float staminaTimer;

    private void Start()
    {
        currentStamina = maxStamina;
    }
    void Update()
    {
        staminaTimer += Time.deltaTime;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        anim.SetFloat("Horizontal", mousePos.x);
        anim.SetFloat("Vertical", mousePos.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
        movementDirection = new Vector3(movement.x, movement.y, 0f).normalized;
        if (Input.GetKeyDown(KeyCode.B) && !dodging && currentStamina >= dodgeCost)
        {
            staminaTimer = 0f;
            StopAllCoroutines();
            Debug.Log("Dodge");
            StartCoroutine(Dodge());
        }
        if(staminaTimer > dodgeRegen && currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 20f;
        }
    }
    void FixedUpdate()
    {
        if(!dodging)
        {
            rb.velocity = (movement.normalized * movementSpeed);
        }
    }
    IEnumerator Dodge()
    {
        currentStamina -= dodgeCost;
        dodging = true;
        int enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, true);

        // Calculate dodge direction based on object's rotation
        Vector2 dodgeDirection = (Vector2)(Quaternion.Euler(0, 0, dodgeOBJ.transform.eulerAngles.z) * Vector2.up);

        // Calculate target position for dodge
        Vector2 dodgeTargetPosition = (Vector2)transform.position + dodgeDirection * dodgeForce;

        float timeElapsed = 0f;

        while (timeElapsed < 0.5f)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, dodgeTargetPosition, timeElapsed / 0.5f));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Re-enable collision with the enemy layer after dodge
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);
        dodging = false;
    }

}