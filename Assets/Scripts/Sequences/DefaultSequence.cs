using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSequence : Sequencer
{
    public PlayerController player;
    public InventoryManager inventoryManager;

    // Update is called once per frame
    public override void ManualUpdate() 
    {
        player.ManualUpdate();
        inventoryManager.ManualUpdate();
    }
}
