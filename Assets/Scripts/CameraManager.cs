using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CAMERA
{
    MAIN, DOOR, NUM
}

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateCamera(CAMERA.MAIN);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCamera(CAMERA.DOOR);
        }
    }

    public void ActivateCamera(CAMERA cam)
    {
        SetCamerasActive(false);
        cameras[(int)cam].gameObject.SetActive(true);
    }

    void SetCamerasActive(bool flag)
    {
        for(int i = 0; i < cameras.Length; ++i)
        {
            cameras[i].gameObject.SetActive(flag);
        }
    }
}
