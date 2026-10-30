using System;
using Placements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Random = UnityEngine.Random;

public class LoreBlock : MonoBehaviour
{
    [SerializeField] private UI UI;
    [SerializeField] private MemoriesManager MemoriesManager;
    [SerializeField] private int layer;
    private string key;
    private PlayerInput _input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] keys = new[]
        {
            "L1-1", "L1-2", "L1-3", "L2-1", "L2-2", "L2-3", "L2-4", "L3-1", "L3-2", "L3-3", "L4-1", "L4-2", "L4-3", "L4-4", "L5-1", "L5-2", "L5-3", "L5-4"
        };
        switch (layer)
        {
            case 0:
                key = keys[Random.Range(0, 3)];
                break;
            case 1:
                key = keys[Random.Range(3, 7)];
                break;
            case 2:
                key = keys[Random.Range(7, 10)];
                break;
            case 3:
                key = keys[Random.Range(10, 14)];
                break;
            default:
                key = keys[Random.Range(14, 18)];
                break;
        }
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        var r = Camera.main.ScreenPointToRay(PlacementManager.Instance.MousePos);
        var b = Physics.Raycast(r, out RaycastHit hit);
        if (b && hit.collider.gameObject == transform.gameObject)
        {
            print(MemoriesManager.Instance.LoreTexts[key]);
            UI.ShowLore(MemoriesManager.Instance.LoreTexts[key]);
        }
    }
    
    private void OnEnable()
    {
        _input = FindAnyObjectByType<PlayerInput>();
        _input.actions["MouseClick"].performed += OnClick;
    }

    private void OnDisable()
    {
        _input.actions["MouseClick"].performed -= OnClick;
    }
}
