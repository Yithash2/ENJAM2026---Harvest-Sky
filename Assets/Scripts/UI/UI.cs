using System;
using System.Collections;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Button NextButton;
    [SerializeField] private Button BuildMenuButton;
    [SerializeField] private TimeCycle _timeCycle;
    [SerializeField] private TextMeshProUGUI RessourceText;
    [SerializeField] private TextMeshProUGUI LoreText;
    [SerializeField] private Image LorePanel;
    [SerializeField] private AddBuildingPlacement FirstBuildingPlacement;
    [SerializeField] private GameObject BuildMenu;
    [SerializeField] private int WidthBetweenBuildings = 50;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Vector3 BuildMenuOpen;
    [SerializeField] private Vector3 BuildMenuClose;
    [SerializeField] private AnimationCurve BuildMenuOppeningCurve;
    [SerializeField] private float BuildMenuOppeningTime;
    private float BuildMenuOppeningTimer;
    private bool BuildMenuOppened;
    private bool BuildMenuMoving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        NextButton.onClick.AddListener(OnNextCivilisationButtonPressed);
        BuildMenuButton.onClick.AddListener(OnBuildButtonPressed);
        
        HideLore();
    }

    // Update is called once per frame
    void Update()
    {
        RessourceText.text = $": {ResourceManager.Instance.Resources}";
    }

    public void OnNextCivilisationButtonPressed()
    {
        _timeCycle.ToggleFastForward();
    }
    
    public void OnBuildButtonPressed()
    {
        print("Je build");
        
        BuildMenuOppeningTimer = 0f;
        if (BuildMenuMoving) return;
        print("Button pressed");
        if (BuildMenuOppened)
        {
            BuildMenuOppened = false;
            StartCoroutine(CloseBuildPanel());
        }
        else
        {
            BuildMenuOppened = true;
            StartCoroutine(OpenBuildPanel());
        }
        
    }

    public void ShowLore(string lore)
    {
        LoreText.text = lore;
        LoreText.color = new Color(LoreText.color.r, LoreText.color.g, LoreText.color.b, 255);
        LorePanel.color = new Color(LorePanel.color.r, LorePanel.color.g, LorePanel.color.b, 204);
        playerInput.actions["MouseClick"].started += _ => HideLore();
    }

    public void HideLore()
    {
        LoreText.color = new Color(LoreText.color.r, LoreText.color.g, LoreText.color.b, 0);
        LorePanel.color = new Color(LorePanel.color.r, LorePanel.color.g, LorePanel.color.b, 0);
    }
    
    public IEnumerator OpenBuildPanel()
    {
        BuildMenuMoving = true;
        while (BuildMenuOppeningTimer < BuildMenuOppeningTime)
        {
            BuildMenuOppeningTimer += Time.deltaTime;
            BuildMenu.transform.position = Vector3.Lerp(BuildMenuClose, BuildMenuOpen, BuildMenuOppeningCurve.Evaluate(BuildMenuOppeningTimer/BuildMenuOppeningTime));
            yield return null;
        }
        BuildMenuMoving = false;
    }
    
    public IEnumerator CloseBuildPanel()
    {
        BuildMenuMoving = true;
        while (BuildMenuOppeningTimer < BuildMenuOppeningTime)
        {
            BuildMenuOppeningTimer += Time.deltaTime;
            BuildMenu.transform.position = Vector3.Lerp(BuildMenuOpen, BuildMenuClose, BuildMenuOppeningCurve.Evaluate(BuildMenuOppeningTimer/BuildMenuOppeningTime));
            yield return null;
        }
        BuildMenuMoving = false;
    }
}
