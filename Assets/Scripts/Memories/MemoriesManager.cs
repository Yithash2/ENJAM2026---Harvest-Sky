using System.Collections.Generic;
using UnityEngine;

public class MemoriesManager : MonoBehaviour
{
    public static MemoriesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public Dictionary<string, string> LoreTexts = new Dictionary<string, string>
    {
        {"test1","lore1"},
        {"test2", "lore2"},
        {"test3", "lore3"}
    };
}
