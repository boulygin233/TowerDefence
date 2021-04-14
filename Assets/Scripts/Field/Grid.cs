using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;
        private float m_NodeSize;
        private Vector3 m_Offset;

        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;

        private Node m_SelectedNode = null;
        

        private FlowFieldPathfinding m_Pathfinding;

        public FlowFieldPathfinding Pathfinding => m_Pathfinding;

        public int Width => m_Width;

        public int Height => m_Height;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target, Vector2Int start)
        {
            m_Width = width;
            m_Height = height;

            m_StartCoordinate = start;
            m_TargetCoordinate = target;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + 0.5f, 0f, j + 0.5f) * nodeSize);
                }
            }

            m_Offset = offset;
            m_NodeSize = nodeSize;
            m_Pathfinding = new FlowFieldPathfinding(this, target, start);
            m_Pathfinding.UpdateField();
        }

        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }

        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }
        
        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }
        
        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }

            return m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    yield return GetNode(i, j);
                }
            }
        }

        public void CheckOccupyability(Vector2Int coordinate)
        {
            Node node = GetNode(coordinate);
            CheckOccupyability(node);
        }

        public Vector2Int GetNodeCoordinate(Node node)
        {
            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    if (node == m_Nodes[i, j])
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }
            return new Vector2Int(0, 0);
        }
        
        public void CheckOccupyability(Node node)
        {
            if (node.m_OccupationAvailability == OccupationAvailability.Undefined && !node.IsOccupied)
            {
                node.IsOccupied = true;
                if (m_Pathfinding.CanOccupy(GetNodeCoordinate(node)))
                {
                    node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
                }
                else
                {
                    node.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
                }
                node.IsOccupied = false;
            }
        }
        
        public Node GetNodeAtPoint(Vector3 point)
        {
            Vector3 difference = point - m_Offset;

            int x = (int) (difference.x / m_NodeSize);
            int z = (int) (difference.z / m_NodeSize);
            Vector2Int coordinate = new Vector2Int(x, z);
            return GetNode(coordinate);
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            List<Node> nodes = new List<Node>();
            foreach (Node node in EnumerateAllNodes())
            {
                float sqrRadius = radius * radius;
                float sqrDistance = (node.Position - point).sqrMagnitude;
                if (sqrDistance <= sqrRadius)
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public bool TryOccupy(Vector2Int coordinate)
        {
            Node node = GetNode(coordinate);
            return TryOccupy(node);
        }
        
        public bool TryOccupy(Node node)
        {
            if (node.m_OccupationAvailability == OccupationAvailability.CanOccupy)
            {
                node.IsOccupied = true;
                node.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
                UpdatePathFinding();
                return true;
            }
            return false;
        }

        public void TryDeoccupy(Vector2Int coordinate)
        {
            Node node = GetNode(coordinate);
            TryDeoccupy(node);
        }
        
        public void TryDeoccupy(Node node)
        {
            if (node.IsOccupied)
            {
                node.IsOccupied = false;
                node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
                UpdatePathFinding();
            }   
        }
        
        public void UpdatePathFinding()
        {
            m_Pathfinding.UpdateField();
        }
    }
}