using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
        
        public List<PlacementObject> BuildingsInCiv;

        public Material MatBase;
        public Material MatDetail;
        public Material MatRuin;

        public SupportBeam supportBeam;

        private PlacementObject CurrentObjectInHand;

        private void OnEnable()
        {
            BuildingsInCiv = new List<PlacementObject>();
            _inputActions.actions["MouseClick"].started += context => Clicked = true;
            _inputActions.actions["MouseClick"].canceled += context => Clicked = false;
        }
        
        private void OnDisable()
        {
            if (_inputActions.actions != null)
            {
                _inputActions.actions["MouseClick"].started -= context => Clicked = true;
                _inputActions.actions["MouseClick"].canceled -= context => Clicked = false;
            }
        }

        public PlacementGrid grid;

        private void FixedUpdate()
        {
            if (CurrentObjectInHand == null) return;
            
            Ray ray = Camera.main.ScreenPointToRay(MousePos);
                
            if(Physics.Raycast(ray, out var hit, 30, PlaceLayer.value))
            {
                var indexs = grid.WorldToGridIndex(hit.point);
                var gridCoord = grid.GridToWorld(indexs.x, indexs.y);
                transform.position = new Vector3(gridCoord.x, 12f, gridCoord.y);
                int layer = grid.ValidObjectDirectPlacement(CurrentObjectInHand, out List<Node> unused, out var _);

                Physics.Raycast(transform.position,Vector3.down,out var info, 20000f,
                    PlaceLayer.value);
                if (layer < 0)
                {
                    transform.position = new Vector3(gridCoord.x, info.point.y + 3f, gridCoord.y);
                }
                else
                {
                    transform.position = new Vector3(gridCoord.x, info.point.y, gridCoord.y);

                    if (Clicked)
                    {
                        CurrentObjectInHand.Place();
                        CurrentObjectInHand = null;
                    } 
                }
                    
            }
        }

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