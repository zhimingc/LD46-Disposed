using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_DisposeChute : MonoBehaviour
{
    public DisposalSequence disposeSequence;
    public EndSequence endSequence;
    public GameObject junkOnFloor;

    private bool heldJunk;

    public void TurnOffJunk()
    {
        heldJunk = junkOnFloor.activeSelf;;
        junkOnFloor.SetActive(false);
    }

    public void EndDispose()
    {
        if (heldJunk)
        {
            disposeSequence.SetState(DisposalSequence.STATE.LOOK_CAPACITY);
        }
        else
        {
            disposeSequence.SetState(DisposalSequence.STATE.END);
        }
    }

    public void SetEndState(EndSequence.STATE newState)
    {
        endSequence.SetState(newState);
    }

    public void SetState(DisposalSequence.STATE newState)
    {
        disposeSequence.SetState(newState);
    }
}
