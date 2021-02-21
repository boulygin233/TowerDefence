using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovementScript : MonoBehaviour
{
    [SerializeField] private float m_Angle_Speed_1;
    [SerializeField] private float m_Angle_Speed_2;
    [SerializeField] private float m_Radius;
    [SerializeField] private float m_Angle_1 = 0f;
    [SerializeField] private float m_Angle_2 = 0f;
    
    // Update is called once per frame
    void Update()
    {
        m_Angle_1 += (m_Angle_Speed_1 / 360 * 2 * Mathf.PI) * Time.deltaTime;
        m_Angle_1 %= (2 * Mathf.PI);
        m_Angle_2 += (m_Angle_Speed_2 / 360 * 2 * Mathf.PI) * Time.deltaTime;
        m_Angle_2 %= (2 * Mathf.PI);
        Vector3 dir;
        dir.x = m_Radius * Mathf.Cos(m_Angle_1) * Mathf.Sin(m_Angle_2) - transform.position.x;
        dir.y = m_Radius * Mathf.Sin(m_Angle_1) * Mathf.Sin(m_Angle_2) - transform.position.y;
        dir.z = m_Radius * Mathf.Cos(m_Angle_2) - transform.position.z;
        transform.Translate(dir);
    }
}