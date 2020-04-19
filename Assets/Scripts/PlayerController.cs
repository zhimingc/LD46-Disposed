using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public SequenceManager sequenceManager;
    public CameraManager cameraManager;
    public Vector3 movePos;
    public float moveSpeed;
    public float rotateSpeed;
    public float targetRotation;

    private bool moveSequence;
    private CAMERA nextCamera;
    private SEQUENCE nextSeq;

    // Start is called before the first frame update
    void Start()
    {
        movePos = transform.position;
    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        UpdateMouseRayPosition();

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
        }

        // rotate robot
        if (Mathf.Abs(transform.rotation.y - targetRotation) > 10.0f)
        {
            //transform.eulerAngles = new Vector3(0, targetRotation, 0);
            //transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0), Space.World);
        }
    }

    void UpdateMouseRayPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 10, Color.yellow);

        if (Input.GetMouseButtonDown(0))
        {
            moveSequence = true;
            nextCamera = CAMERA.NUM;
            nextSeq = SEQUENCE.NUM;

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity, layerMask))
            {
                movePos = hit.point;
                movePos.y = 0;
                Interactable interactScript = hit.transform.GetComponent<Interactable>();

                // specific interactable behaviour
                if (hit.transform.name == "door")
                {
                    nextCamera = CAMERA.DOOR;
                    nextSeq = SEQUENCE.DOOR;

                    if (interactScript)
                    {
                        if (interactScript.setMovePos)
                        {
                            movePos = interactScript.movePos;
                        }
                    }
                }

                // generic
                Vector3 vecToTarget = movePos - transform.position;
                float rotateDir = Vector3.Dot(vecToTarget, transform.forward);
                targetRotation = transform.eulerAngles.y + Mathf.Sign(rotateDir) * Vector3.Angle(vecToTarget, transform.forward);
                transform.LookAt(movePos);
            }            
        }
    }
}
