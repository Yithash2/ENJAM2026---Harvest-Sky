using System;
using UnityEngine;

namespace Placements
{
    using System.Collections.Generic;
    
    public class PlacementObject : MonoBehaviour
    {
        public BoxCollider collider;
        public List<Node> PlacesUsed;

        public int layerPlaced;
        public bool SupportsOtherObjects;
        public bool BadSupport;
        
        public int totalNbrOfNode { get; private set; }

        private void Start()
        {
            totalNbrOfNode = Mathf.RoundToInt(collider.size.x*transform.localScale.x / PlacementManager.Instance.grid.SnapAmount) *
                             Mathf.RoundToInt(collider.size.z*transform.localScale.z / PlacementManager.Instance.grid.SnapAmount);
            print(gameObject.name + totalNbrOfNode);
            Place();
        }

        private void OnValidate()
        {
            collider = GetComponent<BoxCollider>();
            
        }

        [ContextMenu("Place")]
        public void Place()
        {
            bool hasBeenPlaced = PlacementManager.Instance.grid.PlaceObject(this);
            if (hasBeenPlaced)
            {
                if(BadSupport)
                    print($"{gameObject.name} has been placed correctly. But im hanging");
                else
                    print($"{gameObject.name} has been placed correctly.");
            }else 
                print($"{gameObject.name} hasn't been place correctly");
            // TODO : Si le placement est mauvais, faire qqlq
        }

        public int LayerOnTop()
        {
            return layerPlaced + Mathf.FloorToInt(collider.size.y * transform.localScale.y / PlacementManager.Instance.grid.SnapAmount);
        }
        
    }
}