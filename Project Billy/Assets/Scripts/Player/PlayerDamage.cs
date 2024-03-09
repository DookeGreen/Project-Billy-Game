using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static PlayerDamage instance;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void TakeDamage(float damage, float health, GameObject victim)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(victim);
        }
    }
}
