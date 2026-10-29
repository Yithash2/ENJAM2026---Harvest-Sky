using System.Collections;
using System.Resources;
using TMPro;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private bool _test;

    [Header("Data")]
    public bool HasResources;
    public int ResourcesGiven;
    [SerializeField] private float _timeToWait;
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _text;

    void Start()
    {
        _text.text = "-" + ResourcesGiven.ToString();
        HasResources = true;
    }

    private IEnumerator GatherResources()
    {
        yield return new WaitForSeconds(_timeToWait);
        HasResources = true;
    }

    public void OnClick()
    {
        if (HasResources)
        {
            ResourceManager.Instance.AddResources(ResourcesGiven);
            HasResources = false;
            _animator.SetTrigger("Click");
            StartCoroutine(GatherResources());
        }
    }

    // To remove
    void Update()
    {
        if (_test)
        {
            _test = false;
            OnClick();
        }
    }
}
