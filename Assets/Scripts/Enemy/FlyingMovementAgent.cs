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

        public FlyingMovementAgent(float speed, Transform transform, Grid grid)
        {
            m_Speed = speed;
            m_Transform = transform;
            
            SetTargetNode(grid.GetTargetNode());
        }

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            target.y = m_Transform.position.y;

            float distance = (target - m_Transform.position).magnitude;
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
    }
}