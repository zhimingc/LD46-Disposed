using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SEQUENCE
{
    DEFAULT,
    DOOR,
    INCINERATE,
    CHUTE,
    NUM
}

public class SequenceManager : MonoBehaviour
{
    public SEQUENCE currentSequence;
    public List<Sequencer> sequencers;

    public void SetSequence(SEQUENCE nextSequence)
    {
        currentSequence = nextSequence;
    }

    // Update is called once per frame
    void Update()
    {
        sequencers[(int)currentSequence].ManualUpdate();

        switch (currentSequence)
        {
            case SEQUENCE.DEFAULT:
            break;
            case SEQUENCE.DOOR:
            break;
            case SEQUENCE.INCINERATE:
            break;
            case SEQUENCE.CHUTE:
            break;
        }
    }
}
