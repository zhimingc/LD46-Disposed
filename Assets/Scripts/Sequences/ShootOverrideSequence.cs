using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOverrideSequence : Sequencer
{
    public enum STATE 
    {
        SHOOTING,
        HIT,
        ACTIVATE_CHUTE_BUTTON,
        NUM
    }

    public STATE state;
    public GameObject overrideButton;
    public GameObject bulletPrefab;
    public GameObject player;
    public Animator chuteButton;

    // Start is called before the first frame update
    void Start()
    {
        SetState(STATE.SHOOTING);
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {
        switch (state)
        {
            case STATE.SHOOTING:
                if (enterState)
                {
                    enterState = false;
                    chuteButton.gameObject.SetActive(true);

                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = player.transform.position;
                    
                    float flyTime = 0.5f;
                    LeanTween.move(bullet, overrideButton.transform.position, flyTime);
                    Destroy(bullet, flyTime * 2.0f);
                    LeanTween.delayedCall(flyTime, ()=> {
                        SetState(STATE.HIT);
                    });
                }
            break;
            case STATE.HIT:
                if (enterState)
                {
                    enterState = false;
                    overrideButton.GetComponent<Animator>().SetTrigger("pressed");
                }
            break;
            case STATE.ACTIVATE_CHUTE_BUTTON:
                if (enterState)
                {
                    enterState = false;
                    chuteButton.SetTrigger("enter");
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
