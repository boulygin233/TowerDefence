﻿using System;
using UnityEngine;

namespace Field
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;

        [SerializeField] private Vector2Int m_TargetCoordinate;
        [SerializeField] private Vector2Int m_StartCoordinate;
        [SerializeField] private GameObject m_Cursor;
        private Renderer CursorRenderer;
        public Vector2Int targetCoordinate => m_TargetCoordinate;
        public Vector2Int startCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;

        [SerializeField] private float m_NodeSize;

        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;
        
        private void OnValidate()
        {
            m_Camera = Camera.main;

            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
        }

        
        public void CreateGrid()
        {
            m_Camera = Camera.main;

            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
            
            m_Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            CursorRenderer = m_Cursor.GetComponent<Renderer>();
            CursorRenderer.material.SetColor("_Color", Color.green);
        }

        public void RaycastInGrid()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            /*if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);

                if (Input.GetMouseButtonDown(0))
                {
                    Node node = m_Grid.GetNode(x, z);
                    node.IsOccupied = !node.IsOccupied;
                    m_Grid.UpdatePathFinding();
                }
            }*/
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform && hit.transform != m_Cursor.transform)
                {   
                    m_Grid.UnselectNode();
                    m_Cursor.SetActive(false);
                    return;
                }
                
                Vector3 hitPosition = hit.point;
                
                m_Cursor.SetActive(true);
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);
                Vector2Int coordinate = new Vector2Int(x, z);
                Vector3 target = new Vector3((x + 0.5f) * m_NodeSize , 0f, (z + 0.5f) * m_NodeSize )
                                 + m_Offset;
                m_Cursor.transform.position = target;
                
                Node node = m_Grid.GetNode(coordinate);
                m_Grid.CheckOccupyability(coordinate);
                if (node.m_OccupationAvailability == OccupationAvailability.CanOccupy)
                {
                    CursorRenderer.material.SetColor("_Color", Color.green);
                }
                if (node.m_OccupationAvailability == OccupationAvailability.CanNotOccupy)
                {
                    CursorRenderer.material.SetColor("_Color", Color.red);
                }
                
                m_Grid.SelectCoordinate(new Vector2Int(x, z));
                /*if (Input.GetMouseButtonDown(0))
                {
                    if (!m_Grid.TryOccupy(coordinate))
                    {
                        m_Grid.TryDeoccupy(coordinate);
                    }
                }*/
            }
            else
            {
                m_Grid.UnselectNode();
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Grid == null)
            {
                return;
            }

            /*foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }

                if (node.IsOccupied)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(node.Position, 0.2f);
                    continue;
                }

                Gizmos.color = Color.red;
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;
                Vector3 dir = (end - start);

                start -= dir * 0.25f;
                end -= dir * 0.75f;

                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f);
            }*/
            
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