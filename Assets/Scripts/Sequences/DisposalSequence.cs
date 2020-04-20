using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisposalSequence : Sequencer
{
    public enum STATE 
    {
        PRESS_BUTTON,
        SHOOT_BUTTON,
        DISPOSE,
        LOOK_CAPACITY,
        END
    }
    
    public CameraManager cameraManager;
    public TextMeshPro disposeText;
    public MeshRenderer disposeButtonMesh;
    public Color greenPressed;
    public Material pressedMaterial;
    public DisposeChute disposeChute;
    public UseOn disposeButton;
    public Animator capAnim;
    public STATE state;
    public GameObject bulletPrefab;
    public GameObject player;
    public EndSequence endSequence;

    private Color originalRed;
    private Material originalButtonMat;

    // Start is called before the first frame update
    void Start()
    {
        originalRed = disposeText.color;
        originalButtonMat = disposeButtonMesh.material;
    }

    public override void ResetState()
    {
        SetState(STATE.PRESS_BUTTON);
    }

    void PressBehaviour()
    {
        SetDisposeButton(true);
        LeanTween.delayedCall(0.5f, ()=> {
            if (disposeChute.HoldingJunk())
            {
                cameraManager.ActivateCamera(CAMERA.DISPOSE_JUNK);
                LeanTween.delayedCall(1.0f, ()=> {
                    SetState(STATE.DISPOSE);
                });
            }
            else
            {
                SetState(STATE.DISPOSE);
            }
        });        
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {
        switch(state)
        {
            case STATE.PRESS_BUTTON:
                if (disposeButton.behaviour == "shoot") SetState(STATE.SHOOT_BUTTON);
                else
                {
                    PressBehaviour();
                }
            break;
            case STATE.SHOOT_BUTTON:
                if (enterState)
                {
                    enterState = false;
                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = player.transform.position;
                    
                    float flyTime = 0.5f;
                    LeanTween.move(bullet, disposeButtonMesh.transform.position, flyTime);
                    Destroy(bullet, flyTime * 2.0f);
                    LeanTween.delayedCall(flyTime, ()=> {
                        PressBehaviour();
                    });

                    enterState = false;
                }
            break;
            case STATE.DISPOSE:
                if (enterState)
                {
                    disposeChute.disposalAnim.SetTrigger("dispose");
                    enterState = false;
                }
            break;
            case STATE.LOOK_CAPACITY:
                if (enterState)
                {
                    endSequence.readyToEnd = true;
                    cameraManager.ActivateCamera(CAMERA.LOOK_CAPACITY);
                    capAnim.SetTrigger("increase");
                    enterState = false;
                }
            break;
            case STATE.END:
                if (enterState)
                {
                    LeanTween.delayedCall(0.5f, ()=>{
                        SetDisposeButton(false);
                        cameraManager.ActivateCamera(CAMERA.MAIN);
                        sequenceManager.SetSequence(SEQUENCE.DEFAULT);
                    });

                    enterState = false;
                }
            break;
        }
    }

    public void SetDisposeButton(bool flag)
    {
        if (flag)
        {
            disposeText.color = greenPressed;
            disposeButtonMesh.material = pressedMaterial;
        }
        else
        {
            disposeText.color = originalRed;
            disposeButtonMesh.material = originalButtonMat;
        }
    }

    public void SetState(STATE newState)
    {
        enterState = true;
        state = newState;
    }
}
