using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactName;
    public bool setMovePos;
    public Vector3 movePos;
    
    [Header("Items")]
    public bool item;
    public ITEM itemTag;
    public bool removable;
    public Texture itemIcon;
    public string itemName, itemDescript;

    public Texture GetItemIcon()
    {
        return itemIcon;
    }
}
