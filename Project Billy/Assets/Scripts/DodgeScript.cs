using System.Collections;
using UnityEngine;

public class DodgeScript : MonoBehaviour
{
    private Vector2 movement;
    [Range(0.1f, 100f)]
    [SerializeField] private float aimSpeed;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, aimSpeed * Time.deltaTime);
        }        
    }
}
