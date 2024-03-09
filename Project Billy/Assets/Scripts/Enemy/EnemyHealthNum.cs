using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealthNum : MonoBehaviour
{
    [SerializeField] private EnemyHealth hpSO;
    [SerializeField] private Rigidbody2D rb;
    private float hp;
    private float delay;

    private void Awake()
    {
        hp = hpSO.enemyHealth;
    }
    public void TakeDamage(float damage, Vector2 kbRotation, float kbForce, float kbLength)
    {
        delay = kbLength;
        StopAllCoroutines();
        hp -= damage;
        rb.AddForce(kbRotation * kbForce, ForceMode2D.Impulse);
        StartCoroutine(Reset());

        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
    }
}
