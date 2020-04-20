using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject usb, usbBlue;

    // Start is called before the first frame update
    void Start()
    {
        bool unlocked = PlayerPrefs.HasKey("usb");
        usb.SetActive(!unlocked);
        usbBlue.SetActive(unlocked);
    }

    public void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}
