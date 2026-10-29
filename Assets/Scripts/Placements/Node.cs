using System.Collections.Generic;

namespace Placements
{
    public class Node
    {
        public bool Occupied => Objects.Count != 0;

        public List<PlacementObject> Objects = new List<PlacementObject>();//Objects that are currently on the node
    }
}