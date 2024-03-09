using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_Controller : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    [Range(0.1f, 20f)]
    [SerializeField] private float bulletDamage;
    
    [Range(0.1f, 10f)]
    [SerializeField] private float knockback;

    [Range(0.1f, 5f)]
    [SerializeField] private float knockbackDuration;

    [SerializeField] private AudioClip bulletHit;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            col.GetComponent<EnemyHealthNum>().TakeDamage(bulletDamage, transform.up, knockback, knockbackDuration);
            SoundFXManager.instance.PlaySoundFXClip(bulletHit, transform, 1f);
            Destroy(this.gameObject);
        }
    }
}
