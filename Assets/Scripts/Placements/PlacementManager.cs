using System;
using UnityEngine;

namespace Placements
{
    public class PlacementManager : MonoBehaviour
    {
        public static PlacementManager Instance{
            get;
            protected set;
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
}