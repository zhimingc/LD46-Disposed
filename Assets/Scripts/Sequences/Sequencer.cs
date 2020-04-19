using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    protected SequenceManager sequenceManager;
    protected bool enterState;

    public void SetManager(SequenceManager manager)
    {
        sequenceManager = manager;
    }

    // Update is called once per frame
    public virtual void ManualUpdate()
    {
        
    }
}
