using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : Sequencer
{
    public enum STATE 
    {
        TEXT,
        BABY,
        CLOSEUP,
        END
    }

    public STATE state;
    public CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        SetState(STATE.TEXT);
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {
        switch (state)
        {
            case STATE.BABY:
                if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.BABY_INTRO);
                    LeanTween.delayedCall(3.0f, ()=> {
                        SetState(STATE.CLOSEUP);
                    });
                    enterState = false;
                }
            break;
            case STATE.CLOSEUP:
                if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.CLOSE_UP);
                    LeanTween.delayedCall(2.5f, ()=> {
                        SetState(STATE.END);
                    });
                    enterState = false;
                }
            break;
            case STATE.END:
                if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.MAIN);
                    enterState = false;
                }
                if (!cameraManager.IsCameraActive(CAMERA.CLOSE_UP))
                {
                    sequenceManager.SetSequence(SEQUENCE.DEFAULT);
                }
            break;
        }
    }

    public void SetState(STATE newState)
    {
        state = newState;
        enterState = true;
    }
}
