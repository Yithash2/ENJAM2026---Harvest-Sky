namespace Placements
{
    public class Node
    {
        public bool Occupied;
        public bool Blocked;
        public bool Usable => !(this.Occupied && this.Blocked);
    }
}