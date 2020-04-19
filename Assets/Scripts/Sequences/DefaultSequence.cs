using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSequence : Sequencer
{
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void ManualUpdate() 
    {
        player.ManualUpdate();
    }
}
