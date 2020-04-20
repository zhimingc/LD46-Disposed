using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum SEQUENCE
{
    DEFAULT,
    DOOR,
    SHOOT_OVERRIDE,
    CHUTE,
    PLACE_JUNK,
    END,
    INTRO,
    NUM
}

public class SequenceManager : MonoBehaviour
{
    public SEQUENCE currentSequence;
    public List<Sequencer> sequencers;

    private CinemachineImpulseSource cameraImpulseSource;

    private void Awake()
    {
        cameraImpulseSource = GetComponent<CinemachineImpulseSource>();
        for (int i = 0; i < sequencers.Count; i++)
        {
            sequencers[i].SetManager(this);
        }
    }

    private void Start()
    {
        SetSequence(SEQUENCE.INTRO);
    }

    public void SetSequence(SEQUENCE nextSequence)
    {
        currentSequence = nextSequence;
        sequencers[(int)currentSequence].ResetState();
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
            case SEQUENCE.CHUTE:
            break;
        }

        // debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraImpulseSource.GenerateImpulse();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSequence(SEQUENCE.SHOOT_OVERRIDE);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSequence(SEQUENCE.END);
        }
    }

    public void ShakeCamera(Vector3 velocity = new Vector3())
    {
        if (velocity.sqrMagnitude > 0.0f)
        {
            cameraImpulseSource.GenerateImpulse(velocity);
        }
        else
        {
            cameraImpulseSource.GenerateImpulse();
        }
    }
}
