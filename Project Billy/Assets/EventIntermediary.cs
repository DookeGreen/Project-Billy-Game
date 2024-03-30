using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventIntermediary : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyai;

    public void PassOn(){
        Debug.Log("Passed");
        enemyai.TriggerAttack();
    }
}
