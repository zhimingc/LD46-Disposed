using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_ChuteButton : MonoBehaviour
{
    public SequenceManager sequenceManager;

    public void SetSequence(SEQUENCE nextSequence)
    {
        sequenceManager.SetSequence(nextSequence);
    }
}
