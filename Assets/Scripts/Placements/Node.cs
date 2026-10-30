using System.Collections.Generic;

namespace Placements
{
    public class Node
    {
        public bool Blocked;
        public bool Occupied => Objects.Count != 0 || Blocked;

        public List<PlacementObject> Objects = new List<PlacementObject>();//Objects that are currently on the node
    }
}