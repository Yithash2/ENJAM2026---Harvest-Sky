using TMPro;
using UnityEngine;

public class Memory : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private string _textKey;

    void Start()
    {
        DisplayText();
    }
    public void DisplayText()
    {
        _ui.ShowLore(MemoriesManager.Instance.LoreTexts[_textKey]);
    }
}
