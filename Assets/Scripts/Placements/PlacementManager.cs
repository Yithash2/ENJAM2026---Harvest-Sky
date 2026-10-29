using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Placements
{
    public class PlacementManager : MonoBehaviour
    {
        public static PlacementManager Instance{
            get;
            protected set;
        }
        
        [SerializeField] private PlayerInput _inputActions;
        public Vector2 MousePos => _inputActions.actions["MousePos"].ReadValue<Vector2>();
        public LayerMask PlaceLayer;
        public bool Clicked { get; private set; }

        public Material MatBase;
        public Material MatDetail;

        public SupportBeam supportBeam;

        private void OnEnable()
        {
            _inputActions.actions["MouseClick"].started += context => Clicked = true;
            _inputActions.actions["MouseClick"].canceled += context => Clicked = false;
        }
        
        private void OnDisable()
        {
            _inputActions.actions["MouseClick"].started -= context => Clicked = true;
            _inputActions.actions["MouseClick"].canceled -= context => Clicked = false;
        }

        public PlacementGrid grid;

        private void Awake()
        {
            if (Instance != this && Instance !=  null)
            {
                DestroyImmediate(this.gameObject);
            }

            Instance = this;
        }
        
    }
    
    [Serializable]
    public struct SupportBeam
    {
        public GameObject Prefab;

        public void ChangeMaterial(Material MatBase)
        {
            Prefab.GetComponentInChildren<MeshRenderer>().material = MatBase;
        }
    }
}