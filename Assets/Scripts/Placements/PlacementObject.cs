using System;
using UnityEngine;

namespace Placements
{
    
    using System.Collections.Generic;
    public class PlacementObject : MonoBehaviour
    {
        public bool Placed;
        
        public BoxCollider collider;

        public int layerPlaced;
        public bool IsRuined = false;
        public bool BadSupport;

        private List<GameObject> Beams;

        #region Generation

        public GameObject child;
        [SerializeField] private GameObject ruined;
        [ContextMenu("GetRuined")]
        public void GetRuined()
        {
            var go = Instantiate(ruined, transform);
            collider = go.GetComponent<BoxCollider>();
            IsRuined = true;
            DestroyImmediate(child, true);
            child = go;

            foreach (var bea in Beams)
            {
                bea.GetComponentInChildren<MeshRenderer>().material = PlacementManager.Instance.MatRuin;
            }
        }

        #endregion
        
        public int TotalNbrOfNode { get; private set; }

        private PlacementGrid _grid;

        private void Start()
        {
            _grid = PlacementManager.Instance.grid;
            
            TotalNbrOfNode = Mathf.RoundToInt(collider.size.x*transform.localScale.x / _grid.SnapAmount) *
                             Mathf.RoundToInt(collider.size.z*transform.localScale.z / _grid.SnapAmount);
            if(Placed)
                Place();

        }

        private void OnValidate()
        {
            collider = collider?? GetComponent<BoxCollider>();
            child = transform.GetChild(0).gameObject;
        }

        private void FixedUpdate()
        {
            if (Placed)
            {
                
            }
        }
        
        public void Place()
        {
            bool hasBeenPlaced = PlacementManager.Instance.grid.PlaceObject(this);
            if (hasBeenPlaced)
            {
                Placed = true;
                PlaceSupportBeams(collider.bounds.min, collider.bounds.max);
            }
        }

        private void PlaceSupportBeams(Vector3 min, Vector3 max)
        {
            float y = min.y;
            SpawnBeam(min+ new Vector3(0.2f,0,0.2f));
            SpawnBeam(new Vector3(min.x + 0.2f, y, max.z - 0.2f));
            SpawnBeam(new Vector3(max.x - 0.2f, y, max.z - 0.2f));
            SpawnBeam(new Vector3(max.x - 0.2f, y, min.z + 0.2f));
            
        }

        private void SpawnBeam(Vector3 pos)
        {
            var prefab = PlacementManager.Instance.supportBeam;
            prefab.ChangeMaterial(PlacementManager.Instance.MatDetail);
            var beamA= Instantiate(prefab.Prefab, pos, Quaternion.identity);
            beamA.transform.SetParent(transform);
            
            var hit = Physics.Raycast(beamA.transform.position, Vector3.down,out RaycastHit ray, 400000f ,PlacementManager.Instance.PlaceLayer.value);
            
            if (hit)
            {
                if (ray.distance <= 0.0051f)
                {
                    DestroyImmediate(beamA);
                    return;
                }
                beamA.transform.localScale = new Vector3(beamA.transform.localScale.x-0.1f, ray.distance,
                    beamA.transform.localScale.z-0.1f);
                beamA.transform.SetParent(transform);
            }
            else
            {
                DestroyImmediate(beamA);
            }
        }

        public int LayerOnTop()
        {
            return layerPlaced + Mathf.FloorToInt(collider.size.y * transform.localScale.y / PlacementManager.Instance.grid.SnapAmount);
        }
        
    }
}