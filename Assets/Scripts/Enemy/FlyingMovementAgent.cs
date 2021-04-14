using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        private EnemyData m_Data;
        private Node m_CurrentNode;

        private Grid m_Grid;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid, EnemyData data)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = data;
            m_Grid = grid;
            
            SetTargetNode(grid.GetStartNode());
            SetCurrentNode(m_Grid.GetNodeAtPoint(m_Transform.position));
        }

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 position = m_Transform.position;
            
            if (m_Grid.GetNodeAtPoint(position) != m_CurrentNode)
            {
                m_CurrentNode.m_EnemyDatas.Remove(m_Data);
                SetCurrentNode(m_Grid.GetNodeAtPoint(position));
                m_CurrentNode.m_EnemyDatas.Add(m_Data);
            }
            
            Vector3 target = m_TargetNode.Position;
            target.y = position.y;

            float distance = (target - position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Vector3 dir = (target - m_Transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        public void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
        
        public void SetCurrentNode(Node node)
        {
            m_CurrentNode = node;
        }
    }
}