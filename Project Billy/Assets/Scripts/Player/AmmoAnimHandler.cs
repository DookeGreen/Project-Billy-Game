using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoAnimHandler : MonoBehaviour
{
    public bool animDone;

    private void Awake()
    {
        animDone = false;
    }
    void AnimEnd()
    {
        animDone = true;
    }
}
