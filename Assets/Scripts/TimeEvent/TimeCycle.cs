using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeCycle : MonoBehaviour
{
    #region Fields

    [SerializeField] public float timeForDayCycle = 60.0f;
    [SerializeField] private int DayForCivilisationCycle = 3;
    [SerializeField] public TimeState timeState = TimeState.Play;
    [SerializeField] public float FastForwardSpeed = 2.0f;

    public float DayCycleTimer = 0.0f;
    private int DayCounter = 0;
    public int CivilisationCounter = 0;
    public float dtime = 0.0f;

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
        timeState = timeState == TimeState.FastForward ? TimeState.Play : TimeState.FastForward;
    }
    
    #region Events

    public event Action OnDayFullCycle;
    public event Action OnDayNewCycle;
    
    public event Action OnCivilisationFullCycle;
    public event Action OnCivilisationNewCycle;
    
    #endregion
}

public enum TimeState { Pause, Play, FastForward }
