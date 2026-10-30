using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine;
using Placements;
using UnityEngine.Serialization;

public class TimeCycle : MonoBehaviour
{
    #region Fields

    [SerializeField] public float timeForDayCycle = 60.0f;
    [SerializeField] private int DayForCivilisationCycle = 3;
    [SerializeField] public TimeState timeState = TimeState.Play;
    [SerializeField] public float FastForwardSpeed = 2.0f;
    [SerializeField] private AudioClip _fastForwardClip;

    public float DayCycleTimer = 0.0f;
    private int DayCounter = 0;
    public int CivilisationCounter = 0;
    public float dtime = 0.0f;

    public Animator Animator;
    
    #endregion
    
    
    void Start()
    {
        timeForDayCycle *= 100;
    }
    
    void Update()
    {
        dtime = timeState switch
        {
            TimeState.Play => Time.deltaTime,
            TimeState.FastForward => Time.deltaTime * FastForwardSpeed,
            TimeState.Pause => 0f
        };
        DayCycleTimer += dtime;

        if (DayCycleTimer >= timeForDayCycle)
        {
            OnDayFullCycle?.Invoke();
            DayCounter++;
            DayCycleTimer = 0.0f;   
            OnDayNewCycle?.Invoke();
        }
    }

    public void TogglePause()
    {
        timeState = timeState == TimeState.Pause ? TimeState.Play : TimeState.Pause;
    }
    
    public void ToggleFastForward()
    {
        CivilisationCounter++;
        StartCoroutine(FastForward());
    }

    public IEnumerator FastForward()
    {
        SoundManager.Instance.PlaySFX(_fastForwardClip);
        Animator.SetTrigger("Play");

        yield return new WaitForSeconds(3);
        
        PlacementManager.Instance.CurrentCivToRest();
    }
    
    #region Events

    public event Action OnDayFullCycle;
    public event Action OnDayNewCycle;
    
    public event Action OnCivilisationFullCycle;
    public event Action OnCivilisationNewCycle;
    
    #endregion
}

public enum TimeState { Pause, Play, FastForward }
