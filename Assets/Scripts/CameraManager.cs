using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CAMERA
{
    MAIN, 
    DOOR, 
    DOOR_CLOSEUP, 
    DOOR_CLOSEUP_2, 
    BLACK_SCREEN,
    CLOSE_UP,
    SHOULDER_0,
    SHOULDER_1,
    JUNK_FLOOR,
    DISPOSE_JUNK,
    LOOK_CAPACITY,
    BUTTON_SHOT,
    CLOSE_UP_2,
    NOTIFY,
    DOOR_OPEN,
    BABY_INTRO,
    TEXT_INTRO,
    CREDITS,
    NUM
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
    }

    public void FadeIn(CAMERA cam, float fadeTime)
    {
        if (cameras[(int)cam].GetComponent<CinemachineStoryboard>())
        {
            cameras[(int)cam].GetComponent<CinemachineStoryboard>().m_Alpha = 0.0f;
            LeanTween.value(0.0f, 1.0f, fadeTime).setOnUpdate((float val)=>{
                cameras[(int)cam].GetComponent<CinemachineStoryboard>().m_Alpha = val;
            });
        } 
    }

    public void ActivateCamera(CAMERA cam, bool fade = false, float fadeTime = 2.0f)
    {
        SetCamerasActive(false);
        cameras[(int)cam].gameObject.SetActive(true);
        if (fade && cameras[(int)cam].GetComponent<CinemachineStoryboard>())
        {
            cameras[(int)cam].GetComponent<CinemachineStoryboard>().m_Alpha = 1.0f;
            LeanTween.value(1.0f, 0.0f, fadeTime).setOnUpdate((float val)=>{
                cameras[(int)cam].GetComponent<CinemachineStoryboard>().m_Alpha = val;
            });
        }
    }

    public bool IsCameraActive(CAMERA cam)
    {
        return CinemachineCore.Instance.IsLive(cameras[(int)cam]);
    }

    void SetCamerasActive(bool flag)
    {
        for(int i = 0; i < cameras.Length; ++i)
        {
            cameras[i].gameObject.SetActive(flag);
        }
    }
}
