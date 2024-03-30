using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Range(0.1f, 10f)]
    [SerializeField] private float speed;
    [Range(0.1f, 10f)]
    [SerializeField] private float range;
    [Range(0.1f, 10f)]
    [SerializeField] private float Knockback;
    [Range(0.1f, 10f)]
    [SerializeField] private float knockbackDuration;
    [Range(0.1f, 10f)]
    [SerializeField] private float damage;
    [Range(0.1f, 10f)]
    [SerializeField] private float delay;
    [Range(10f, 50f)]
    [SerializeField] private float ActiveDistance;
    [Range(0f, 90f)]
    [SerializeField] private float angleOffset;
    [Range(0f, 90f)]
    [SerializeField] private float circleRadius;
    [Range(0f, 90f)]
    [SerializeField] private float angleInterval;
    [Range(0.1f, 10f)]
    [SerializeField] private float attackRange;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject parent;
    [SerializeField] private Transform player;
    [SerializeField] private AudioClip atkHit;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private BoolScriptableObject iframe;
    [SerializeField] private Animator animator; // Reference to the Animator component
    [SerializeField] private State currentState;
    private Vector2 moveDirection;
    private bool canAttack = true;
    private float distance;
    private bool LOS = false;
    private Vector3 destination;
    private Vector2 collisionPoint1;
    private Vector2 collisionPoint2;
    private float deAgroTimer;

    private enum State
    {
        Idle,
        Chasing,
        Attacking
    }

    private void Start()
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        if(animator.GetBool("IsAttacking") == false && animator.GetBool("IsWalking") == true)
        {
            canAttack = true;
        }
        moveDirection = (destination - transform.position).normalized;
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);
        distance = Vector3.Distance(transform.position, player.position);
        if(distance <= attackRange)
        {
            currentState = State.Attacking;
        }
        else if (distance <= range)
        {
            animator.SetBool("IsAttacking", false);
            deAgroTimer = 0f;
            Vector3 directionToPlayer = player.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, range, obstacleLayer);
            if (hit.collider != null)
            {
                transform.localPosition = new Vector3(0f, 0.5f, 0f);
                LOS = false;
                if (currentState == State.Chasing)
                {
                    NewRay(directionToPlayer);
                }
            }
            else
            {
                transform.localPosition = new Vector3(0f, 0f, 0f);
                LOS = true;
            }
            if (LOS)
            {
                currentState = State.Chasing;
                destination = transform.position + directionToPlayer.normalized;
            }
            Debug.DrawRay(transform.position, directionToPlayer.normalized * 10f, Color.green);
        }
        else if (distance > range && currentState == State.Chasing || !LOS && currentState == State.Chasing)
        {
            deAgroTimer += Time.deltaTime;
            if (deAgroTimer >= 5f)
            {
                currentState = State.Idle;
            }
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("IsWalking", false);
                rb.velocity = Vector3.zero;
                break;

            case State.Chasing:
                animator.SetBool("IsWalking", true);
                rb.velocity = moveDirection * speed;
                break;

            case State.Attacking:
                animator.SetBool("IsWalking", false);
                rb.velocity = Vector3.zero;
                Attack();
                break;
        }
    }

    private void NewRay(Vector3 direction)
    {
        float angle1 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < 90; i++)
        {
            angle1 += angleInterval;
            angle2 -= angleInterval;

            Vector2 thisDirection1 = new Vector2(Mathf.Cos(angle1 * Mathf.Deg2Rad), Mathf.Sin(angle1 * Mathf.Deg2Rad));
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, thisDirection1.normalized, range, obstacleLayer);

            if (hit1.collider == null)
            {
                Vector2 thisDirectionFin1 = new Vector2(Mathf.Cos((angle1 + angleOffset) * Mathf.Deg2Rad), Mathf.Sin((angle1 + angleOffset) * Mathf.Deg2Rad));
                destination = transform.position + new Vector3(thisDirectionFin1.normalized.x, thisDirectionFin1.normalized.y, 0);
                break;
            }
            else
            {
                collisionPoint1 = hit1.point;
            }

            Vector2 thisDirection2 = new Vector2(Mathf.Cos(angle2 * Mathf.Deg2Rad), Mathf.Sin(angle2 * Mathf.Deg2Rad));
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, thisDirection2.normalized, range, obstacleLayer);
            if (hit2.collider == null)
            {
                Vector2 thisDirectionFin2 = new Vector2(Mathf.Cos((angle2 - angleOffset) * Mathf.Deg2Rad), Mathf.Sin((angle2 - angleOffset) * Mathf.Deg2Rad));
                destination = transform.position + new Vector3(thisDirectionFin2.normalized.x, thisDirectionFin2.normalized.y, 0);
                break;
            }
            else
            {
                collisionPoint2 = hit2.point;
            }
        }
    }

    public void TriggerAttack()
    {
        Debug.Log("Recieved");
        // Play attack sound
        SoundFXManager.instance.PlaySoundFXClip(atkHit, player.transform, 1f);

        // Apply damage to the player
        player.GetComponent<PlayerHealth>().TakeDamage(damage, transform.up, Knockback, knockbackDuration);

        // Reset attack animation state
        EndAttackAnimation();
    }

    private void Attack()
    {
        if (canAttack)
        {
            animator.SetBool("IsAttacking", true);
            canAttack = false;
        }
    }

    // Animation Event: Triggered at the end of the attack animation
    public void EndAttackAnimation()
    {
        animator.SetBool("IsAttacking", false);
        currentState = State.Idle;
        StartCoroutine(Wait());
    }
    private IEnumerator Wait(){
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
}
