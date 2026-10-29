using System.Collections;
using System.Resources;
using TMPro;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private bool _test;
    public int ID;

    [Header("Data")]
    [SerializeField] private float _timeToWait;
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _text;

    // private IEnumerator GatherResources()
    // {
    //     yield return new WaitForSeconds(_timeToWait);
    //     HasResources = true;
    // }

    public void OnClick()
    {
        int resourcesGiven = Random.Range(1, 4) * 10;
        _text.text = "+" + resourcesGiven.ToString();
        ResourceManager.Instance.AddResources(resourcesGiven, ID);
        _animator.SetTrigger("Click");
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
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
