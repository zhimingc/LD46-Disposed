using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyCine : MonoBehaviour
{
    public GameObject notifyText;
    public MeshRenderer notifyBase;
    public MeshRenderer notifyLight;

    [Header("References")]
    public Material lightOn, baseOn;

    // Start is called before the first frame update
    void Start()
    {
        notifyText.SetActive(false);    
    }

    public void Activate()
    {
        notifyText.SetActive(true);
        notifyBase.material = baseOn;
        notifyLight.material = lightOn;
    }
}
