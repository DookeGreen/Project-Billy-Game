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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip atkHit;

    private bool canAttack = true;
    private float distance;

    void Update()
    {
        
        distance = Vector2.Distance(parent.transform.position, player.transform.position);
        if(distance < ActiveDistance)
        {   
            Vector2 direction = player.transform.position - parent.transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            if(distance < range)
            {
                if(canAttack)
                {
                    StopAllCoroutines();
                    StartCoroutine(Attack());
                }
            }
            else
            {
                parent.transform.position = Vector2.MoveTowards(parent.transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }}
    private IEnumerator Attack()
    {
        float randNum = Random.Range(-2,2);
        canAttack = false;
        player.GetComponent<PlayerHealth>().TakeDamage(damage, transform.up, Knockback, knockbackDuration);
        SoundFXManager.instance.PlaySoundFXClip(atkHit, player.transform, 1f);
        if(randNum >= 0)
        {
            rb.AddForce(transform.up * 10f, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }
        else if(randNum < 0)
        {
            rb.AddForce(-transform.up * 10f, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
    }
}
