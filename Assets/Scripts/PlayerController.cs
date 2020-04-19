using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMOTES
{
    DEFAULT,
    SMILE,
    SHOCK,
    PANIC,
    SAD,
    BLINK,
    NUM
}

public class PlayerController : MonoBehaviour
{
    public SequenceManager sequenceManager;
    public InventoryManager inventoryManager;
    public CameraManager cameraManager;
    public Vector3 movePos;
    public float moveSpeed;
    public float rotateSpeed;
    public float targetRotation;

    public MeshRenderer robotFace;
    public List<Texture> emoteSprites;
    public GameObject leftArm;
    public Baby babyScript;

    [Header("Blinking")]
    public Vector2 blinkFrequency;
    public float blinkCloseTime;
    private float blinkTimer, blinkTime;

    [Header("Arm Cannon")]
    public GameObject armCannon;
    public GameObject armSparks;

    private EMOTES currentEmote;
    private bool moveSequence;
    private CAMERA nextCamera;
    private SEQUENCE nextSeq;
    private Interactable pickingUpItem;
    private bool pausePicking;
    private bool droppingUSB;
    private bool hasLeftArm;

    // Start is called before the first frame update
    void Start()
    {
        movePos = transform.position;
        hasLeftArm = true;
    }

    public void SetPicking(bool flag)
    {
        pausePicking = flag;
    }

    public bool HasLeftArm()
    {
        return hasLeftArm;
    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        if (!pausePicking) UpdateMouseRayPosition();

        // translate robot
        if (Vector3.Distance(transform.position, movePos) > 0.1f)
        {
            Vector3 vecToTarget = movePos - transform.position;
            transform.Translate(vecToTarget.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
        else if (moveSequence)
        {
            moveSequence = false;
            if (nextCamera != CAMERA.NUM) cameraManager.ActivateCamera(nextCamera);
            if (nextSeq != SEQUENCE.NUM) sequenceManager.SetSequence(nextSeq);
            if (pickingUpItem != null) 
            {
                if (pickingUpItem.removable) pickingUpItem.gameObject.SetActive(false);
                inventoryManager.AddItem(pickingUpItem);
            }
            if (droppingUSB)
            {
                babyScript.droppedUSB.SetActive(true);
                droppingUSB = false;
                pausePicking = false;
            }
        }

        // emotes
        UpdateBlink();

        // DEBUG KEYS
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //currentEmote = (EMOTES) (((int) currentEmote + 1) % (int) EMOTES.NUM);
            //robotFace.material.mainTexture = emoteSprites[(int)currentEmote];
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DeactivateLeftArm();
        }

    }

    void UpdateBlink()
    {
        blinkTime += Time.deltaTime;
        if (blinkTime > blinkTimer)
        {
            if (currentEmote != EMOTES.BLINK)
            {
                SetEmote(EMOTES.BLINK);
                blinkTimer = blinkCloseTime;
            }
            else
            {
                SetEmote(EMOTES.DEFAULT);
                blinkTimer = Random.Range(blinkFrequency.x, blinkFrequency.y);
            }
            blinkTime = 0;
        }
    }

    public void SetEmote(EMOTES emote)
    {
        currentEmote = emote;
        robotFace.material.mainTexture = emoteSprites[(int)emote];
    }

    public void DeactivateLeftArm()
    {
        leftArm.SetActive(false);
        hasLeftArm = false;
    }

    void ResetPostMoveBehaviour()
    {
        nextCamera = CAMERA.NUM;
        nextSeq = SEQUENCE.NUM;
        pickingUpItem = null;
    }

    void UpdateMouseRayPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 10, Color.yellow);

        if (Input.GetMouseButtonDown(0))
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.tag != "Interact") return;
                
                ResetPostMoveBehaviour();
                moveSequence = true;
                movePos = hit.point;
                movePos.y = 0;
                Interactable interactScript = hit.transform.GetComponent<Interactable>();

                // specific interactable behaviour
                if (interactScript)
                {
                    if (interactScript.setMovePos)
                    {
                        movePos = interactScript.movePos;
                    }

                    // item interactions
                    if (interactScript.item)
                    {
                        pickingUpItem = interactScript;
                    }
                }
                if (hit.transform.name == "door")
                {
                    nextCamera = CAMERA.DOOR;
                    nextSeq = SEQUENCE.DOOR;
                }

                // generic
                Vector3 vecToTarget = movePos - transform.position;
                float rotateDir = Vector3.Dot(vecToTarget, transform.forward);
                targetRotation = transform.eulerAngles.y + Mathf.Sign(rotateDir) * Vector3.Angle(vecToTarget, transform.forward);
                transform.LookAt(movePos);
            }            
        }
    }

    public void DropUSBByBaby()
    {
        movePos = new Vector3(-6.25f, 0, 5.5f);
        Vector3 vecToTarget = movePos - transform.position;
        float rotateDir = Vector3.Dot(vecToTarget, transform.forward);
        targetRotation = transform.eulerAngles.y + Mathf.Sign(rotateDir) * Vector3.Angle(vecToTarget, transform.forward);
        transform.LookAt(movePos);

        moveSequence = true;
        droppingUSB = true;
        pausePicking = true;
    }

    public void AddCannon()
    {
        armSparks.SetActive(false);
        armCannon.SetActive(true);
    }
}
