using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Contoller : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [Range(0.1f, 10f)]
    [SerializeField] private float dodgeForce;
    [Range(0.1f, 10f)]
    [SerializeField] private float dodgeTime;
    [SerializeField] private string enemyLayerName;
    [SerializeField] private GameObject dodgeOBJ;
    [Range(50f, 250f)]
    [SerializeField] private float maxStamina;
    [Range(0f, 250f)]
    [SerializeField] private float dodgeCost;
    [Range(0f, 20f)]
    [SerializeField] private float dodgeRegen;
    [SerializeField] private BoxCollider2D box1;
    [SerializeField] private BoxCollider2D box2;
    private float currentStamina;
    private Vector2 mousePos;
    private Vector2 movement;
    private Vector3 lastPosition;
    private Vector3 movementDirection;
    private int count;
    [SerializeField] private BoolScriptableObject iframe;
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

        // Calculate dodge direction based on object's rotation
        Vector2 dodgeDirection = (Vector2)(Quaternion.Euler(0, 0, dodgeOBJ.transform.eulerAngles.z) * Vector2.up);

        // Calculate target position for dodge
        Vector2 dodgeTargetPosition = (Vector2)transform.position + dodgeDirection * dodgeForce;

        float timeElapsed = 0f;

        while (timeElapsed < dodgeTime)
        {
            if(timeElapsed > dodgeTime/4 && timeElapsed < 2 * (dodgeTime/3))
            {
                iframe.iframe = true;
                sr.color = new Color(0, 0, 0);
            }
            else
            {
                iframe.iframe = false;
                sr.color = new Color(255, 255, 255);
            }
            rb.MovePosition(Vector2.Lerp(transform.position, dodgeTargetPosition, timeElapsed / 0.5f));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Re-enable collision with the enemy layer after dodge
        dodging = false;
    }

}