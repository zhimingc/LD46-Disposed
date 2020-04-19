using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_Robot : MonoBehaviour
{
    public DoorSequence doorSequence;
 
    public void SetDoorSequence(DoorSequence.STATE newState)
    {
        doorSequence.SetState(newState);
    }
}
