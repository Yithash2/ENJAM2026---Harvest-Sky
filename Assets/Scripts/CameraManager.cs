using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private PlayerInput _inputActions;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _scrollMultiplier;

    [Header("Scene")]
    [SerializeField] private Transform _center;
    public float Height;

    private bool _isMovingCam;
    private float _minHeight;
    private TimeCycle _timeCycle;
    

    private void OnEnable()
    {
        _timeCycle = FindAnyObjectByType<TimeCycle>();
        //_inputActions.actions["MoveCam"].started += OnStartHoldCamera();
        _inputActions.actions["MoveCam"].started += context => _isMovingCam = true;
        _inputActions.actions["MoveCam"].performed += context => _isMovingCam = true;
        _inputActions.actions["MoveCam"].canceled += context => _isMovingCam = false;

    }

    private void OnDisable()
    {
        _inputActions.actions["MoveCam"].started -= context => _isMovingCam = true;
        _inputActions.actions["MoveCam"].performed -= context => _isMovingCam = true;
        _inputActions.actions["MoveCam"].canceled -= context => _isMovingCam = false;
    }

    void Start()
    {
        _minHeight = transform.position.y;
    }

    void Update()
    {
        Height = _timeCycle.CivilisationCounter * 2.5f;
        if (_isMovingCam)
        {
            transform.RotateAround(_center.position, Vector3.up, _inputActions.actions["CameraPosition"].ReadValue<float>() * _rotationSpeed * Time.deltaTime * 2f);
        }

        float scroll = _inputActions.actions["Scroll"].ReadValue<float>();
        if (scroll < 0f && transform.position.y >= _minHeight || scroll > 0f && transform.position.y <= _minHeight + Height)
        {
            transform.Translate(new Vector3(0f, scroll * _scrollMultiplier * Time.deltaTime * 2f, 0f));
        }
    }
}
