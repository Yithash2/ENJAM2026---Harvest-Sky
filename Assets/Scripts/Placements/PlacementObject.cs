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

        #region Generation

        private GameObject child;
        [SerializeField] private GameObject ruined;
        [ContextMenu("GetRuined")]
        public void GetRuined()
        {
            var go = Instantiate(ruined, transform);
            collider = go.GetComponent<BoxCollider>();
            IsRuined = true;
            DestroyImmediate(child, true);
        }

        #endregion
        
        public int TotalNbrOfNode { get; private set; }

        private PlacementGrid _grid;

        private void Start()
        {
            _grid = PlacementManager.Instance.grid;
            
            TotalNbrOfNode = Mathf.RoundToInt(collider.size.x*transform.localScale.x / _grid.SnapAmount) *
                             Mathf.RoundToInt(collider.size.z*transform.localScale.z / _grid.SnapAmount);
            print(gameObject.name + TotalNbrOfNode);
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
            else
            {   
                Ray ray = Camera.main.ScreenPointToRay(PlacementManager.Instance.MousePos);
                
                if(Physics.Raycast(ray, out var hit, 30, PlacementManager.Instance.PlaceLayer.value))
                {
                    var indexs = _grid.WorldToGridIndex(hit.point);
                    var gridCoord = _grid.GridToWorld(indexs.x, indexs.y);
                    transform.position = new Vector3(gridCoord.x, 12f, gridCoord.y);
                    int layer = PlacementManager.Instance.grid.ValidObjectDirectPlacement(this, out List<Node> unused, out var _);

                    Physics.Raycast(transform.position,Vector3.down,out var info, 20000f,
                        PlacementManager.Instance.PlaceLayer.value);
                    if (layer < 0)
                    {
                        transform.position = new Vector3(gridCoord.x, info.point.y + 3f, gridCoord.y);
                    }
                    else
                    {
                        
                        transform.position = new Vector3(gridCoord.x, info.point.y, gridCoord.y);

                        if (PlacementManager.Instance.Clicked)
                        {
                            Place();
                        } 
                    }
                    
                }
                
            }
        }
        
        public void Place()
        {
            bool hasBeenPlaced = PlacementManager.Instance.grid.PlaceObject(this);
            if (hasBeenPlaced)
            {
                Placed = true;
                PlaceSupportBeams(collider.bounds.min, collider.bounds.max);
                if(BadSupport)
                    print($"{gameObject.name} has been placed correctly. But im hanging");
                else
                    print($"{gameObject.name} has been placed correctly.");
            }else 
                print($"{gameObject.name} hasn't been place correctly");
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
            
            var hit = Physics.Raycast(beamA.transform.position, Vector3.down,out RaycastHit ray, 400000f ,PlacementManager.Instance.PlaceLayer.value);
            
            if (hit)
            {
                if (ray.distance <= 0.00051f)
                {
                    DestroyImmediate(beamA);
                    return;
                }
                beamA.transform.localScale = new Vector3(beamA.transform.localScale.x-0.1f, ray.distance,
                    beamA.transform.localScale.z-0.1f);
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