using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenCine : MonoBehaviour
{
    public GameObject door;
    public EndSequence endSequence;

    public void Activate()
    {
        LeanTween.moveLocalY(door, 4.0f, 3.0f);
        LeanTween.delayedCall(3.0f, ()=> {
            endSequence.SetState(EndSequence.STATE.TO_MENU);
        });
    }
}
