using System;
using UnityEngine;

namespace Field
{
    public class MovementCursor : MonoBehaviour
    {
        [SerializeField] private MovementAgent m_MovementAgent;
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;
        [SerializeField] private GameObject m_Cursor;
        [SerializeField] private float m_NodeSize;
        [SerializeField] private Vector3 m_Position;

        private Camera m_Camera;

        private Vector3 m_Offset;

        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);
            transform.position = m_Position;
            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
        }

        private void Awake()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);
            transform.position = m_Position;
            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
            
            m_Camera = Camera.main;
            m_Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            var coursorRenderer = m_Cursor.GetComponent<Renderer>();
            coursorRenderer.material.SetColor("_Color", Color.blue);
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {   
                    //m_Cursor.SetActive(false);
                    return;
                }
                
                Vector3 hitPosition = hit.point;
                
                m_Cursor.SetActive(true);
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);
                Debug.Log(x + " " + z);
                Vector3 target = new Vector3((x + 0.5f) * m_NodeSize , 0f, (z + 0.5f) * m_NodeSize )
                                 + m_Offset;
                m_Cursor.transform.position = target;
                if (Input.GetMouseButtonDown(0))
                {
                    target.y += 0.5f;
                    m_MovementAgent.SetTarget(target);
                }
            }
            else
            {
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < m_GridWidth + 1; i++)
            {
                Vector3 from = m_Offset + (new Vector3(m_NodeSize * i, 0f, 0f));
                Vector3 to = from;
                to.z += m_NodeSize * m_GridHeight;
                Gizmos.DrawLine(from, to);
            }
            Gizmos.color = Color.cyan;
            for (int i = 0; i < m_GridHeight + 1; i++)
            {
                Vector3 from = m_Offset + (new Vector3(0f, 0f, m_NodeSize * i));
                Vector3 to = from;
                to.x += m_NodeSize * m_GridWidth;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}