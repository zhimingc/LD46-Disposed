using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DoorSequence : Sequencer
{
    public enum STATE
    {
        EXTEND_ARM,
        REACHING_GAME,
        BUTTON_PRESSED,
        ROBOT_CLOSEUP,
        ROBOT_GETUP,
        RETURN
    }

    public GameObject doorObj;
    public CameraManager cameraManager;
    public CinemachineVirtualCamera mainCam, closeUpCam;
    public Animator robotArm, robotHand;
    public GameObject robotPalmObj;
    public AnimScript_VFX impactEffect;
    public GameObject closingText;
    public STATE state;
    public Animator robotAnim;
    public GameObject robotArmSparks;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = robotAnim.GetComponent<PlayerController>();
        SetState(STATE.EXTEND_ARM);
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {
        switch (state)
        {
            case STATE.EXTEND_ARM:
                // disable door interactivity
                doorObj.layer = 0;

                if (!CinemachineCore.Instance.IsLive(mainCam))
                {
                    robotArm.gameObject.SetActive(true);
                    robotArm.SetTrigger("slide");                
                }
            break;
            case STATE.REACHING_GAME:
                if (Input.GetMouseButtonDown(0))
                {
                    robotHand.enabled = false;
                    robotPalmObj.transform.localPosition = new Vector3(-0.375f, 0, -1.3f);
                    impactEffect.Activate();

                    // check if the button is hit
                    float handRotation = robotHand.transform.localEulerAngles.x - 360.0f;
                    if (handRotation < -30.0f && handRotation > -42.0f)
                    {
                        SetState(STATE.BUTTON_PRESSED);
                    }
                }

                if (impactEffect.EffectActive() == false)
                {
                    robotPalmObj.transform.localPosition = new Vector3(-0.45f, 0, -1.3f);
                    robotHand.enabled = true;
                }
            break;
            case STATE.BUTTON_PRESSED:
                if (enterState)
                {
                    enterState = false;
                    closingText.SetActive(true);
                }
            break;
            case STATE.ROBOT_CLOSEUP:
                if (enterState)
                {
                    enterState = false;
                    robotArm.gameObject.SetActive(false);
                    robotAnim.SetTrigger("door_closeup");
                    robotAnim.GetComponent<PlayerController>().SetEmote(EMOTES.SMILE);
                    robotAnim.transform.eulerAngles = Vector3.zero;
                    cameraManager.ActivateCamera(CAMERA.DOOR_CLOSEUP);

                    float zoomTime = 1.5f;
                    LeanTween.moveLocalY(closeUpCam.gameObject, 1.0f, zoomTime);

                    LeanTween.delayedCall(zoomTime, ()=> {
                        playerController.SetEmote(EMOTES.SHOCK);
                        sequenceManager.ShakeCamera();

                        float shakeTime = 1.0f;
                        LeanTween.delayedCall(shakeTime, ()=> {
                            SetState(STATE.ROBOT_GETUP);
                        });
                    });
                }
            break;
            case STATE.ROBOT_GETUP:
                if (enterState)
                {
                    enterState = false;
                    robotAnim.SetTrigger("door_getup");
                    cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);
                    playerController.SetEmote(EMOTES.SAD);
                    playerController.DeactivateLeftArm();
                    robotArmSparks.SetActive(true);
                    doorObj.transform.localPosition = new Vector3(0, -5.0f, -3.75f);

                    LeanTween.delayedCall(0.5f, ()=> {
                        cameraManager.ActivateCamera(CAMERA.DOOR_CLOSEUP_2);
                    });
                }
            break;
            case STATE.RETURN:
                if (enterState)
                {
                    enterState = false;
                    cameraManager.ActivateCamera(CAMERA.MAIN);
                    sequenceManager.SetSequence(SEQUENCE.DEFAULT);
                    robotAnim.transform.eulerAngles = new Vector3(0, 180.0f, 0);
                }
            break;
        }    
    }

    public void SetState(STATE newState)
    {
        state = newState;
        enterState = true;
    }

    public void TriggerHandExtend()
    {
        robotHand.SetTrigger("extend");
    }
}
