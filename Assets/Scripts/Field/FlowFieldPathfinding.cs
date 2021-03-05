using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    struct Connection
    {
        private Vector2Int coordinate;
        private float weight;

        public Connection(Vector2Int coordinate, float weight)
        {
            this.coordinate = coordinate;
            this.weight = weight;
        }

        public Vector2Int Coordinate => coordinate;

        public float Weight => weight;
    }

    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;

        public FlowFieldPathfinding(Grid grid, Vector2Int target, Vector2Int start)
        {
            m_Grid = grid;
            m_Target = target;
            m_Start = start;
        }

        public bool CanOccupy(Vector2Int coordinate)
        {
            bool getToStart = false;

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).IsVisited = true;
            
            while (queue.Count > 0 && !getToStart)
            {
                Vector2Int current = queue.Dequeue();
                if (current == m_Start)
                {
                    getToStart = true;
                }
                else
                {
                    foreach (Vector2Int neighbour in GetNeighboursForCheck(current))
                    {
                        m_Grid.GetNode(neighbour).IsVisited = true;
                        queue.Enqueue(neighbour);
                    }
                }
            }

            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.IsVisited = false;
            }

            return getToStart;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
                if (node.m_OccupationAvailability != OccupationAvailability.CanNotOccupy)
                {
                    node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
                }
            }

            m_Grid.GetNode(m_Start).m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
            m_Grid.GetNode(m_Target).m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);

            m_Grid.GetNode(m_Target).PathWeight = 0f;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    float weightToTarget = currentNode.PathWeight + neighbour.Weight;
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour.Coordinate);
                    }
                }
            }
            Node nextNode = m_Grid.GetNode(m_Start).NextNode;
            while (nextNode != m_Grid.GetNode(m_Target))
            {
                nextNode.m_OccupationAvailability = OccupationAvailability.Undefined;
                nextNode = nextNode.NextNode;
            }
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;
            Vector2Int upRightCoordinate = coordinate + Vector2Int.up + Vector2Int.right;
            Vector2Int upLeftCoordinate = coordinate + Vector2Int.up + Vector2Int.left;
            Vector2Int downRightCoordinate = coordinate + Vector2Int.down + Vector2Int.right;
            Vector2Int downLeftCoordinate = coordinate + Vector2Int.down + Vector2Int.left;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied;
            bool hasUpRightNode = hasRightNode && hasUpNode && !m_Grid.GetNode(upRightCoordinate).IsOccupied;
            bool hasUpLeftNode = hasUpNode && hasLeftNode && !m_Grid.GetNode(upLeftCoordinate).IsOccupied;
            bool hasDownRightNode = hasDownNode && hasRightNode && !m_Grid.GetNode(downRightCoordinate).IsOccupied;
            bool hasDownLeftNode = hasDownNode && hasLeftNode && !m_Grid.GetNode(downLeftCoordinate).IsOccupied;

            if (hasDownNode)
            {
                Connection neighbour = new Connection(downCoordinate, 1f);
                yield return neighbour;
            }

            if (hasLeftNode)
            {
                Connection neighbour = new Connection(leftCoordinate, 1f);
                yield return neighbour;
            }

            if (hasRightNode)
            {
                Connection neighbour = new Connection(rightCoordinate, 1f);
                yield return neighbour;
            }

            if (hasUpNode)
            {
                Connection neighbour = new Connection(upCoordinate, 1f);
                yield return neighbour;
            }

            if (hasDownLeftNode)
            {
                Connection neighbour = new Connection(downLeftCoordinate, Mathf.Sqrt(2f));
                yield return neighbour;
            }

            if (hasDownRightNode)
            {
                Connection neighbour = new Connection(downRightCoordinate, Mathf.Sqrt(2f));
                yield return neighbour;
            }

            if (hasUpLeftNode)
            {
                Connection neighbour = new Connection(upLeftCoordinate, Mathf.Sqrt(2f));
                yield return neighbour;
            }

            if (hasUpRightNode)
            {
                Connection neighbour = new Connection(upRightCoordinate, Mathf.Sqrt(2f));
                yield return neighbour;
            }
        }

        private IEnumerable<Vector2Int> GetNeighboursForCheck(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;
            Vector2Int upRightCoordinate = coordinate + Vector2Int.up + Vector2Int.right;
            Vector2Int upLeftCoordinate = coordinate + Vector2Int.up + Vector2Int.left;
            Vector2Int downRightCoordinate = coordinate + Vector2Int.down + Vector2Int.right;
            Vector2Int downLeftCoordinate = coordinate + Vector2Int.down + Vector2Int.left;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied &&
                                !m_Grid.GetNode(rightCoordinate).IsVisited;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied &&
                               !m_Grid.GetNode(leftCoordinate).IsVisited;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied &&
                             !m_Grid.GetNode(upCoordinate).IsVisited;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied &&
                               !m_Grid.GetNode(downCoordinate).IsVisited;
            bool hasUpRightNode = hasRightNode && hasUpNode && !m_Grid.GetNode(upRightCoordinate).IsOccupied &&
                                  !m_Grid.GetNode(upRightCoordinate).IsVisited;
            bool hasUpLeftNode = hasUpNode && hasLeftNode && !m_Grid.GetNode(upLeftCoordinate).IsOccupied &&
                                 !m_Grid.GetNode(upLeftCoordinate).IsVisited;
            bool hasDownRightNode = hasDownNode && hasRightNode && !m_Grid.GetNode(downRightCoordinate).IsOccupied &&
                                    !m_Grid.GetNode(downRightCoordinate).IsVisited;
            bool hasDownLeftNode = hasDownNode && hasLeftNode && !m_Grid.GetNode(downLeftCoordinate).IsOccupied &&
                                   !m_Grid.GetNode(downLeftCoordinate).IsVisited;

            if (hasDownNode)
            {
                yield return downCoordinate;
            }

            if (hasLeftNode)
            {
                yield return leftCoordinate;
            }

            if (hasRightNode)
            {
                yield return rightCoordinate;
            }

            if (hasUpNode)
            {
                yield return upCoordinate;
            }

            if (hasDownLeftNode)
            {
                yield return downLeftCoordinate;
            }

            if (hasDownRightNode)
            {
                yield return downRightCoordinate;
            }

            if (hasUpLeftNode)
            {
                yield return upLeftCoordinate;
            }

            if (hasUpRightNode)
            {
                yield return upRightCoordinate;
            }
        }
    }
}