using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_CloseButton : MonoBehaviour
{
    public DoorSequence doorSequence;

    public void SetState(DoorSequence.STATE newState)
    {
        doorSequence.SetState(newState);
    }
}
