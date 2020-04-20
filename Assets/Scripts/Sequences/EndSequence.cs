using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSequence : Sequencer
{
    public enum STATE 
    {
        BUTTON_SHOT,
        CAMERA_ZOOM,
        CAP_FULL,
        NOTIFY,
        END,
        TO_MENU
    }

    public STATE state;
    public CameraManager cameraManager;
    public Vector3 playerPos, playerRot;
    public GameObject bulletPrefab;
    public GameObject disposeButton;
    public GameObject player;
    public DisposalSequence disposalSequence;
    public Animator chuteAnim;
    public GameObject chuteFloor;
    public GameObject zoomCamera;
    public Animator capAnim;
    public NotifyCine notifyScript;
    public DoorOpenCine doorCine;
    public GameObject creditsObj;
    public PlayerController playerController;

    public bool readyToEnd;
    private bool camMove;

    float blackScreenDelay = 1.0f;
    float fadeTime = 2.5f;
    float fadeSit = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        readyToEnd = false;
        SetState(STATE.BUTTON_SHOT);
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {
        switch (state)
        {
            case STATE.BUTTON_SHOT:
                if (enterState)
                {
                    cameraManager.ActivateCamera(CAMERA.BUTTON_SHOT);
                    camMove = true;
                    enterState = false;
                }

                if (camMove && !cameraManager.IsCameraActive(CAMERA.MAIN))
                {
                    camMove = false;
                    player.transform.position = playerPos;
                    player.transform.eulerAngles = playerRot;
                    player.transform.parent = chuteFloor.transform;

                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = player.transform.position;
                    
                    float flyTime = 1.0f;
                    LeanTween.move(bullet, disposeButton.transform.position, flyTime);
                    Destroy(bullet, flyTime * 2.0f);
                    LeanTween.delayedCall(flyTime, ()=> {
                        disposalSequence.SetDisposeButton(true);
                        LeanTween.delayedCall(0.75f, ()=> {
                            cameraManager.ActivateCamera(CAMERA.CLOSE_UP_2);
                            SetState(STATE.CAMERA_ZOOM);                            
                        });
                    });
                }
            break;
            case STATE.CAMERA_ZOOM:
                if (enterState)
                {
                    float lookTime = 2.5f;
                    float zoomTime = 3.5f;
                    LeanTween.delayedCall(lookTime, ()=>{
                        chuteAnim.SetTrigger("dispose");
                        LeanTween.moveX(zoomCamera, 15.0f, zoomTime).setEaseInQuad();
                        LeanTween.delayedCall(zoomTime, ()=>{
                            cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);
                            if (readyToEnd) SetState(STATE.CAP_FULL);
                            else SetState(STATE.TO_MENU);
                        });
                    });
                    enterState = false;
                }
            break;
            case STATE.CAP_FULL:
                if (enterState)
                {
                    LeanTween.delayedCall(blackScreenDelay, ()=>{
                        cameraManager.ActivateCamera(CAMERA.LOOK_CAPACITY, true, fadeTime);
                        LeanTween.delayedCall(fadeTime + fadeSit, ()=>{
                            capAnim.SetTrigger("max");
                        });
                    });
                    enterState = false;
                }
            break;
            case STATE.NOTIFY:
                if (enterState)
                {
                    LeanTween.delayedCall(fadeSit, ()=> {
                        cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);

                        LeanTween.delayedCall(blackScreenDelay, ()=>{
                            cameraManager.ActivateCamera(CAMERA.NOTIFY, true, fadeTime);
                            LeanTween.delayedCall(fadeTime + fadeSit, ()=>{
                                notifyScript.Activate();
                                LeanTween.delayedCall(1.0f, ()=> {
                                    SetState(STATE.END);
                                });
                            });
                        });
                    });

                    enterState = false;
                }
            break;
            case STATE.END:
                if (enterState)
                {
                    LeanTween.delayedCall(fadeSit, ()=> {
                        cameraManager.ActivateCamera(CAMERA.BLACK_SCREEN);
                        disposalSequence.SetDisposeButton(false);
                        if (playerController.putUSB)
                        {
                            PlayerPrefs.SetString("usb", "true");
                        }
                        
                        LeanTween.delayedCall(blackScreenDelay, ()=>{
                            cameraManager.ActivateCamera(CAMERA.DOOR_OPEN, true, fadeTime);
                            LeanTween.delayedCall(fadeTime + fadeSit, ()=>{
                                doorCine.Activate();
                            });
                        });
                    });
                    enterState = false;
                }
            break;
            case STATE.TO_MENU:
                if (enterState)
                {
                    LeanTween.delayedCall(1.0f, ()=> {
                        cameraManager.FadeIn(CAMERA.DOOR_OPEN, 2.5f);
                        LeanTween.delayedCall(2.5f, ()=> {
                            cameraManager.ActivateCamera(CAMERA.CREDITS);
                            creditsObj.SetActive(true);
                            LeanTween.delayedCall(10.0f, ()=> {
                                SceneManager.LoadScene(0);
                            });
                        });
                    });
                    enterState = false;
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
