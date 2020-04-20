using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposeChute : MonoBehaviour
{
    public Animator disposalAnim;
    public GameObject junk;

    public bool HoldingJunk()
    {
        return junk.activeSelf;
    }
}
