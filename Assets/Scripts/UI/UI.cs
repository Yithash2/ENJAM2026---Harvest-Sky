using System.Collections;
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
    private bool BuildMenuOppening;
    private Coroutine BuildMenuCoroutine;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirstBuildingPlacement.WidthBetweenBuildings = WidthBetweenBuildings;
        
        foreach (var buildingType in BuildingTypes) {
            FirstBuildingPlacement.Add(buildingType);
        }

        NextButton.onClick.AddListener(OnNextCivilisationButtonPressed);
        NextButton.onClick.AddListener(OnBuildButtonPressed);
        
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
        print("Oppening Panel !");
        BuildMenuOppening = !BuildMenuOppening;
        BuildMenuCoroutine = StartCoroutine(OpenBuildPanel());
    }

    public IEnumerator OpenBuildPanel()
    {
        Vector3 newPos;
        if (BuildMenuOppening)
        {
            BuildMenuOppeningTimer += Time.deltaTime;
            newPos = Vector3.Lerp(BuildMenuClose, BuildMenuOpen, BuildMenuOppeningCurve.Evaluate(BuildMenuOppeningTimer/BuildMenuOppeningTime));
            
        }
        else
        {
            BuildMenuOppeningTimer -= Time.deltaTime;
            newPos = Vector3.Lerp(BuildMenuClose, BuildMenuOpen, BuildMenuOppeningCurve.Evaluate(BuildMenuOppeningTimer/BuildMenuOppeningTime));
        }
        
        BuildMenu.transform.position = newPos;
        if (BuildMenuOppeningTimer > BuildMenuOppeningTime || BuildMenuOppeningTimer < 0)
            StopCoroutine(BuildMenuCoroutine);
        else
            yield break;
    }
}
