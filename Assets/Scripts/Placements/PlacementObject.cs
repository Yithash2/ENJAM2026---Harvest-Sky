using System;
using UnityEngine;

namespace Placements
{
    using System.Collections.Generic;
    
    public class PlacementObject : MonoBehaviour
    {
        
        public List<Node> PlacesUsed;

        private void Start()
        {
            Place();
        }

        public void Place()
        {
            PlacementManager.Instance.grid.PlaceObject(this);
        }
    }
}