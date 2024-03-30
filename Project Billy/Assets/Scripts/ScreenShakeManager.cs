using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager instance;

    [SerializeField] private CinemachineVirtualCamera shakeCam;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void PlayScreenShake(Transform spawnTransform, float duration)
    {
        CinemachineVirtualCamera shakeFX = Instantiate(shakeCam, spawnTransform.position, Quaternion.identity);

        Destroy(shakeFX.gameObject, duration);
    }
}
