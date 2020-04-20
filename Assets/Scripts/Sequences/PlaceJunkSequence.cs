using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceJunkSequence : Sequencer
{
    public enum STATE 
    {
        CLOSE_UP,
        JUNK_0,
        JUNK_1,
        ON_FLOOR,
        END
    }

    public CameraManager cameraManager;
    public PlayerController playerController;
    public STATE state;
    public GameObject beforeJunk, afterJunk;
    public GameObject scribbleSign;

    public List<Vector3> sequencePositions, sequenceRotations;

    private float cutTime = 0.5f, stayTime = 1.5f;
    private bool doneState = false;
    public bool isInCloseUp;

    // Start is called before the first frame update
    void Start()
    {
        SetState(STATE.CLOSE_UP);
    }

    void SetPlayerTransform(int index)
    {
        playerController.transform.position = sequencePositions[index];
        playerController.transform.eulerAngles = sequenceRotations[index];
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {
        if (doneState == true) return;

        switch (state)
        {
            case STATE.CLOSE_UP:
                if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.CLOSE_UP);
                    enterState = false;
                }

                if (!cameraManager.IsCameraActive(CAMERA.MAIN))
                {
                    playerController.SetEmote(EMOTES.BULB);
                    doneState = true;
                    LeanTween.delayedCall(stayTime, ()=>{
                        cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);
                        SetPlayerTransform(1);
                        LeanTween.delayedCall(cutTime, ()=> {
                            playerController.SetEmote(EMOTES.SMILE);
                            SetState(STATE.JUNK_0);
                        });
                    });
                }
            break;
            case STATE.JUNK_0:
                if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.SHOULDER_1);
                    LeanTween.delayedCall(stayTime, ()=>{
                        cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);
                        SetPlayerTransform(0);
                        LeanTween.delayedCall(cutTime, ()=> {
                            SetState(STATE.JUNK_1);
                        });
                    });
                    enterState = false;
                }            
            break;
            case STATE.JUNK_1:
            if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.SHOULDER_0);
                    LeanTween.delayedCall(stayTime, ()=>{
                        cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);
                        SetPlayerTransform(2);
                        beforeJunk.SetActive(false);
                        afterJunk.SetActive(true);
                        LeanTween.delayedCall(cutTime, ()=> {
                            SetState(STATE.ON_FLOOR);
                        });
                    });                   
                    enterState = false;
                }
            break;
            case STATE.ON_FLOOR:
            if (enterState)
                {
                    isInCloseUp = false;
                    cameraManager.ActivateCamera(CAMERA.JUNK_FLOOR);
                    LeanTween.delayedCall(stayTime, ()=>{
                        cameraManager.ActivateCamera(CAMERA.CLOSE_UP);
                        isInCloseUp = true;
                    });   
                    enterState = false;
                }

                if (isInCloseUp && !cameraManager.IsCameraActive(CAMERA.JUNK_FLOOR))
                {
                    doneState = true;
                    LeanTween.delayedCall(stayTime, ()=> {
                        SetState(STATE.END);
                    });
                }
            break;
            case STATE.END:
            if (enterState)
                {
                    scribbleSign.SetActive(true);
                    cameraManager.ActivateCamera(CAMERA.MAIN);
                    sequenceManager.SetSequence(SEQUENCE.DEFAULT);
                    enterState = false;
                }
            break;
        }
    }

    public void SetState(STATE newState)
    {
        state = newState;
        doneState = false;
        enterState = true;
    }
}
