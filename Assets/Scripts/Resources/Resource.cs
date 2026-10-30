using System;
using System.Collections;
using System.Resources;
using Placements;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Resource : MonoBehaviour
{
    [SerializeField] private bool _test;
    public int ID;

    [Header("Data")]
    [SerializeField] private float _timeToWait;
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AudioClip[] _clips;
    private PlayerInput _input;

    private void OnEnable()
    {
        _input = FindAnyObjectByType<PlayerInput>();
        _input.actions["MouseClick"].performed += OnClick;
    }

    private void OnDisable()
    {
        _input.actions["MouseClick"].performed -= OnClick;
    }

    public void Interact()
    {
        PlaySound();
        int resourcesGiven = UnityEngine.Random.Range(1, 4) * 10;
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
            Interact();
        }
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(_input.actions["MousePos"].ReadValue<Vector2>()); // Input.mousePosition
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider == GetComponent<BoxCollider>())
            {
                Interact();
            }
        }
    }

    void PlaySound()
    {
        SoundManager.Instance.PlaySFX(_clips[UnityEngine.Random.Range(0, 2)]);
    }
}
