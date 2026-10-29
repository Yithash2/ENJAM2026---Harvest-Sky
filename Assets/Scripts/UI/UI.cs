using System.Collections;
using System.Timers;
using TMPro;
using UnityEngine;
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
    [SerializeField] private Canvas BuildMenu;
    [SerializeField] private GameObject[] BuildingTypes;
    [SerializeField] private int WidthBetweenBuildings = 50;

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
        FirstBuildingPlacement.WidthBetweenBuildings = WidthBetweenBuildings;
        
        foreach (var buildingType in BuildingTypes) {
            FirstBuildingPlacement.Add(buildingType);
        }

        NextButton.onClick.AddListener(OnNextCivilisationButtonPressed);
        BuildMenuButton.onClick.AddListener(OnBuildButtonPressed);
        
        LoreText.color = new Color(LoreText.color.r, LoreText.color.g, LoreText.color.b, 0);
        LorePanel.color = new Color(LorePanel.color.r, LorePanel.color.g, LorePanel.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNextCivilisationButtonPressed()
    {
        _timeCycle.ToggleFastForward();
    }
    
    public void OnBuildButtonPressed()
    {
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

    public IEnumerator OpenBuildPanel()
    {
        BuildMenuMoving = true;
        Vector3 newPos;
        while (BuildMenuOppeningTimer < BuildMenuOppeningTime)
        {
            BuildMenuOppeningTimer += Time.deltaTime;
            newPos = Vector3.Lerp(BuildMenuClose, BuildMenuOpen, BuildMenuOppeningCurve.Evaluate(BuildMenuOppeningTimer/BuildMenuOppeningTime));
            BuildMenu.transform.localPosition = newPos;
            yield return null;
        }
        BuildMenuMoving = false;
    }
    
    public IEnumerator CloseBuildPanel()
    {
        BuildMenuMoving = true;
        Vector3 newPos;
        while (BuildMenuOppeningTimer < BuildMenuOppeningTime)
        {
            BuildMenuOppeningTimer += Time.deltaTime;
            newPos = Vector3.Lerp(BuildMenuOpen, BuildMenuClose, BuildMenuOppeningCurve.Evaluate(BuildMenuOppeningTimer/BuildMenuOppeningTime));
            BuildMenu.transform.localPosition = newPos;
            yield return null;
        }
        BuildMenuMoving = false;
    }
}
