using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript_RobotArm : MonoBehaviour
{
    public DoorSequence doorSequence;

    public void TriggerHandExtend()
    {
        doorSequence.TriggerHandExtend();
    }

    public void SetState(DoorSequence.STATE newState)
    {
        doorSequence.SetState(newState);
    }
}
