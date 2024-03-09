using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class gun_Aim : MonoBehaviour
{
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] CinemachineVirtualCamera ShakeFX;
    [Range(0.1f, 2f)]
    [SerializeField] private float ShakeDur;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;
    [Range(1, 8)]
    [SerializeField] private int maxBullets = 6;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject bar;
    [SerializeField] Sprite[] sprites;
    [SerializeField] AmmoAnimHandler ammoanim;

    Image image = null;
    private int currentBullets;
    private Vector2 mousePos;
    private float fireTimer;
    private int reloadPlay;

    private void Start()
    {
        anim.enabled = false;
        currentBullets = maxBullets;
        ShakeFX.Priority = 9;
        image = bar.GetComponent<Image>();
    }
    private void Update()
    {
        AnimDecide();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        anim.SetInteger("Bullets", currentBullets);
        if (ammoanim.animDone)
        {
            currentBullets = maxBullets;
            fireTimer = 0;
            anim.enabled = false;
            reloadPlay = 0;
            ammoanim.animDone = false;
        }
        if (Input.GetMouseButtonDown(0) && fireTimer <= 0f && currentBullets != 0)
        {
            Shoot();
            fireTimer = fireRate;
        }
        else if (currentBullets == 0)
        {
            anim.enabled = true;
            Reload();
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
    }
    private void Shoot()
    {
        SoundFXManager.instance.PlaySoundFXClip(shootSFX, transform, 1f);
        StopAllCoroutines();
        StartCoroutine(Shake(ShakeDur));
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        currentBullets -=1;
    }
    void AnimDecide()
    {
        if(currentBullets == 1)
        {
            image.sprite = (sprites[0]);
        }
        else if(currentBullets == 2)
        {
            image.sprite = (sprites[1]);
        }
        else if(currentBullets == 3)
        {
            image.sprite = (sprites[2]);
        }
        else if(currentBullets == 4)
        {
            image.sprite = (sprites[3]);
        }
        else if(currentBullets == 5)
        {
            image.sprite = (sprites[4]);
        }
        else if(currentBullets == 6)
        {
            image.sprite = (sprites[5]);
        }
    }
    IEnumerator Shake(float t)
    {
        ShakeFX.Priority = 11;
        yield return new WaitForSeconds(t);
        ShakeFX.Priority = 9;
    }
    private void Reload()
    {
        if(reloadPlay == 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(reloadSFX, transform, 1f);
            reloadPlay += 1;
        }
    }
}
