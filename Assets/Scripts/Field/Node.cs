using UnityEngine;

namespace Field
{
    
    public enum OccupationAvailability
    {
        CanOccupy,
        CanNotOccupy,
        Undefined
    }
    
    public class Node
    {
        public Vector3 Position;
        
        public Node NextNode;
        public bool IsOccupied;
        public OccupationAvailability m_OccupationAvailability;

        public float PathWeight;
        public bool IsVisited;

        public Node(Vector3 position)
        {
            Position = position;
            m_OccupationAvailability = OccupationAvailability.Undefined;
            IsVisited = false;
        }

        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}