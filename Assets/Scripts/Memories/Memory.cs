using TMPro;
using UnityEngine;

public class Memory : MonoBehaviour
{
    [SerializeField] private TMP_Text _displayText;
    [SerializeField] private string _textKey;

    void Start()
    {
        DisplayText();
    }
    public void DisplayText()
    {
        _displayText.text = MemoriesManager.Instance.LoreTexts[_textKey];
    }
}
