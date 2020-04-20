using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_IntroText : MonoBehaviour
{
    public IntroSequence introSequence;

    public void SetState(IntroSequence.STATE newState)
    {
        introSequence.SetState(newState);
    }
}
