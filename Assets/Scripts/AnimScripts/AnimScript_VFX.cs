using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_VFX : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool EffectActive()
    {
        return GetComponent<MeshRenderer>().enabled;
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().enabled = true;
        animator.enabled = true;
    }

    public void TurnOffEffect()
    {
        animator.enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }
    
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
