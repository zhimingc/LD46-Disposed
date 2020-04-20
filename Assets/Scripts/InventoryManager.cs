using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ITEM
{
    METAL_TUBE,
    GARBAGE,
    JUNK,
    USB,
    UP_GARBAGE_CANNON,
    P_GARBAGE_CANNON,
    MEMORY_USB,
    NUM
}

public enum USE_ON
{
    SELF,
    BABY,
    OVERRIDE_BUTTON,
    CHUTE_BUTTON,
    CHUTE_FLOOR
}

public class InventoryManager : MonoBehaviour
{
    public SequenceManager sequenceManager;
    public PlayerController playerController;
    public List<Slot> slots;
    public GameObject itemTextParent;
    public TextMeshPro itemNameText, itemDescText;
    public List<Texture> combinedItemIcons;
    
    [Header("Middle Text")]
    public GameObject middleTextObj;
    public TextMeshPro item1Text, usageText, item2Text;
    public GameObject middleText2;

    private GameObject draggedObj;
    private Vector3 draggedOrigin;
    private float distFromCam;

    private void Awake()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            slots[i].index = i;
            slots[i].itemTag = ITEM.NUM;
        }
    }

    private int FindFreeSlot()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i].itemTag == ITEM.NUM)
            {
                return i;
            }
        }
        return 0;
    }

    public void AddItem(Interactable item)
    {
        int slotIndex = FindFreeSlot();
        Slot currentSlot = slots[slotIndex];
        currentSlot.gameObject.SetActive(true);
        currentSlot.GetComponent<BoxCollider>().enabled = false;
        currentSlot.itemName = item.itemName;
        currentSlot.itemDescript = item.itemDescript;
        currentSlot.itemTag = item.itemTag;
        GameObject iconObj = slots[slotIndex].icon;
        iconObj.GetComponent<MeshRenderer>().material.mainTexture = item.itemIcon;

        Vector3 origin = iconObj.transform.position;
        iconObj.transform.position = item.transform.position;
        float moveTime = 1.0f;
        LeanTween.move(iconObj, origin, moveTime);
        LeanTween.delayedCall(moveTime, ()=>{
            currentSlot.GetComponent<BoxCollider>().enabled = true;
        });
    }

    void RemoveItem(int index)
    {
        Slot currentSlot = slots[index];
        currentSlot.gameObject.SetActive(false);
        currentSlot.itemTag = ITEM.NUM;
    }

    public void CombineItem(ITEM newItem)
    {
        int currentItem = FindFreeSlot();
        Slot currentSlot = slots[currentItem];
        currentSlot.gameObject.SetActive(true);
        currentSlot.GetComponent<BoxCollider>().enabled = true;
        currentSlot.itemTag = newItem;
        GameObject iconObj = slots[currentItem].icon;

        switch (newItem)
        {
            case ITEM.UP_GARBAGE_CANNON:
                currentSlot.itemName = "Unpowered Garbage Cannon";
                currentSlot.itemDescript = "Propels human garbage, requires power source.";
                iconObj.GetComponent<MeshRenderer>().material.mainTexture = combinedItemIcons[0];
            break;
            case ITEM.P_GARBAGE_CANNON:
                currentSlot.itemName = "Garbage Cannon";
                currentSlot.itemDescript = "Propels human garbage, powered by me.";
                iconObj.GetComponent<MeshRenderer>().material.mainTexture = combinedItemIcons[1];
            break;
            case ITEM.MEMORY_USB:
                currentSlot.itemName = "My Memories";
                currentSlot.itemDescript = "Created recently.";
                iconObj.GetComponent<MeshRenderer>().material.mainTexture = combinedItemIcons[2];
            break;
        }
    }

    public void ManualUpdate()
    {
        UpdateMouseRayPosition();
        UpdateDragged();
    }

    void SetDragged(bool flag, Slot obj = null)
    {
        middleTextObj.SetActive(flag);

        if (flag)
        {
            itemTextParent.SetActive(false);
            item1Text.text = obj.itemName;
            draggedObj = obj.gameObject;
            draggedOrigin = obj.transform.position;
            distFromCam = (obj.transform.position - Camera.main.transform.position).magnitude;
            draggedObj.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            draggedObj.transform.position = draggedOrigin;
            draggedObj.GetComponent<BoxCollider>().enabled = true;
            draggedObj = null;
        }
    }

    void SetHover(Slot obj)
    {
        bool flag = obj != null;
        
        itemTextParent.SetActive(flag);

        if (flag)
        {
            itemNameText.text = obj.itemName;
            itemDescText.text = obj.itemDescript;
        }
    }

    void UpdateDragged()
    {
        if (draggedObj != null)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                draggedObj.transform.position = Camera.main.transform.position + mouseRay.direction * distFromCam;
            }         
        }
    }

    void SetUIVisible(bool flag)
    {
        itemTextParent.SetActive(flag);
        middleTextObj.SetActive(flag);
    }

    void UpdateMouseRayPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        if (Input.GetMouseButtonDown(0))
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.tag != "Slot") return;

                SetDragged(true, hit.transform.GetComponent<Slot>());
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity, layerMask))
            {
                usageText.text = "";
                item2Text.text = "";
                if (hit.transform.tag == "Slot")
                {
                    usageText.text = "USE WITH ";
                    item2Text.text = hit.transform.GetComponent<Slot>().itemName + "?";
                }

                if (hit.transform.tag == "UseOn" || hit.transform.GetComponent<UseOn>())
                {
                    //itemDescText.text = "USE ON " + hit.transform.GetComponent<UseOn>().useName + "?";
                    usageText.text = "USE ON ";
                    item2Text.text = hit.transform.GetComponent<UseOn>().useName + "?";
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (draggedObj == null) return;

            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.tag == "Slot")
                {
                    // check if valid use
                    Slot slotScript = draggedObj.GetComponent<Slot>();
                    Slot slot2 = hit.transform.GetComponent<Slot>();
                    if (slotScript && 
                        (slotScript.itemTag == ITEM.METAL_TUBE && slot2.itemTag == ITEM.GARBAGE) ||
                        (slotScript.itemTag == ITEM.GARBAGE && slot2.itemTag == ITEM.METAL_TUBE))
                    {
                        RemoveItem(slotScript.index);
                        RemoveItem(slot2.index);
                        CombineItem(ITEM.UP_GARBAGE_CANNON);
                    }
                }

                UseOn useScript = hit.transform.GetComponent<UseOn>();
                if (useScript != null)
                {
                    Slot slotScript = draggedObj.GetComponent<Slot>();
                    if (slotScript)
                    {
                        if (slotScript.itemTag == ITEM.USB && useScript.useTag == USE_ON.SELF)
                        {
                            RemoveItem(slotScript.index);
                            CombineItem(ITEM.MEMORY_USB);
                        }

                        if (slotScript.itemTag == ITEM.MEMORY_USB && useScript.useTag == USE_ON.BABY)
                        {
                            RemoveItem(slotScript.index);
                            playerController.DropUSBByBaby();
                        }

                        if (slotScript.itemTag == ITEM.UP_GARBAGE_CANNON && useScript.useTag == USE_ON.SELF &&
                            playerController.HasLeftArm() == false)
                        {
                            RemoveItem(slotScript.index);
                            CombineItem(ITEM.P_GARBAGE_CANNON);
                            playerController.AddCannon();
                        }

                        if (slotScript.itemTag == ITEM.P_GARBAGE_CANNON)
                        {
                            if (useScript.useTag == USE_ON.OVERRIDE_BUTTON)
                            {
                                sequenceManager.SetSequence(SEQUENCE.SHOOT_OVERRIDE);
                                SetUIVisible(false);                                
                            }

                            if (useScript.useTag == USE_ON.CHUTE_BUTTON)
                            {
                                if (playerController.lastInteract == "ChuteFloor")
                                {
                                    sequenceManager.SetSequence(SEQUENCE.END);
                                }
                                else
                                {
                                    sequenceManager.SetSequence(SEQUENCE.CHUTE);
                                }
                                useScript.behaviour = "shoot";                                
                            }
                        }

                        if (slotScript.itemTag == ITEM.JUNK && useScript.useTag == USE_ON.CHUTE_FLOOR)
                        {
                            RemoveItem(slotScript.index);
                            sequenceManager.SetSequence(SEQUENCE.PLACE_JUNK);
                        }
                    }

                }
            }

            if (draggedObj != null)
            {
                SetDragged(false);
            }           
        }
        else
        {
            Slot slotHovered = null;
            middleText2.SetActive(false);
            
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.tag == "Slot")
                {
                    slotHovered = hit.transform.GetComponent<Slot>();
                }
                if (hit.transform.tag == "Scribble")
                {
                    middleText2.SetActive(true);
                }
            }

            SetHover(slotHovered);
        }
    }

}
