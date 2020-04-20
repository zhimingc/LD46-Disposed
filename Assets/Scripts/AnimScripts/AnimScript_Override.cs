using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_Override : MonoBehaviour
{
    public ShootOverrideSequence sequence;

    public void SetState(ShootOverrideSequence.STATE newState)
    {
        sequence.SetState(newState);
    }
}
