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
            bool b = PlacementManager.Instance.grid.PlaceObject(this);
            Debug.Log(b);
        }
    }
}