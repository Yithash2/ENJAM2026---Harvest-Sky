using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine;
using Placements;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimeCycle : MonoBehaviour
{
    #region Fields

    [SerializeField] public float timeForDayCycle = 60.0f;
    [SerializeField] private int DayForCivilisationCycle = 3;
    [SerializeField] public TimeState timeState = TimeState.Play;
    [SerializeField] public float FastForwardSpeed = 2.0f;
    [SerializeField] private AudioClip _fastForwardClip;
    [SerializeField] private Button[] _endInteractions;
    [SerializeField] private GameObject _quitBtn;
    
    public static TimeCycle Instance{
        get;
        protected set;
    }

    public float DayCycleTimer = 0.0f;
    private int DayCounter = 0;
    public int CivilisationCounter = 0;
    public float dtime = 0.0f;
    private bool _canFastForward = true;

    [SerializeField] private Material BaseMat;
    [SerializeField] private Material DetailMat;

    [SerializeField] private ColorAndHint[] BaseColors;
    [SerializeField] private ColorAndHint[] DetailColors;

    [Serializable]
    struct ColorAndHint
    {
        public Color Item1;
        public Color Item2;
    }

    public Animator Animator;
    
    #endregion
    
    
    void Start()
    {
        timeForDayCycle *= 100;
        BaseMat.SetColor("_Color",BaseColors[CivilisationCounter].Item1);
        BaseMat.SetColor("_Color_1",BaseColors[CivilisationCounter].Item2);
        DetailMat.SetColor("_Color", DetailColors[CivilisationCounter].Item1);
        DetailMat.SetColor("_Color_1", DetailColors[CivilisationCounter].Item2);
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
        if (_canFastForward)
        {
            CivilisationCounter++;
            _canFastForward = false;
            StartCoroutine(FastForward());
        }
    }

    public IEnumerator FastForward()
    {
        SoundManager.Instance.PlaySFX(_fastForwardClip);
        Animator.SetTrigger("Play");

        yield return new WaitForSeconds(3);

        _canFastForward = true;
        PlacementManager.Instance.CurrentCivToRest();
        PlacementManager.Instance.grid.SpawnRelik();
        if (CivilisationCounter == 4)
        {
            foreach (var btn in _endInteractions)
            {
                btn.interactable = false;
            }
            _quitBtn.SetActive(true);
        }
        if (CivilisationCounter >= BaseColors.Length) CivilisationCounter = BaseColors.Length - 1;
        
        BaseMat.SetColor("_Color",BaseColors[CivilisationCounter].Item1);
        BaseMat.SetColor("_Color_1",BaseColors[CivilisationCounter].Item2);
        DetailMat.SetColor("_Color", DetailColors[CivilisationCounter].Item1);
        DetailMat.SetColor("_Color_1", DetailColors[CivilisationCounter].Item2);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    private void Awake()
    {
        if (Instance != this && Instance !=  null)
        {
            DestroyImmediate(this.gameObject);
        }

        Instance = this;
    }
    
    #region Events

    public event Action OnDayFullCycle;
    public event Action OnDayNewCycle;
    
    public event Action OnCivilisationFullCycle;
    public event Action OnCivilisationNewCycle;
    
    #endregion
}

public enum TimeState { Pause, Play, FastForward }
