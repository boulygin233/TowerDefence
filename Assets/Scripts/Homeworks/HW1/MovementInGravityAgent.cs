using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInGravityAgent : MonoBehaviour
{
    [SerializeField] private Vector3 m_Speed;
    [SerializeField] private float m_Weight;
    [SerializeField] private Vector3 m_Start;
    [SerializeField] private float m_Diam;
    [SerializeField] private float gravity_Weight;
    [SerializeField] private float gravity_G;
    [SerializeField] private Vector3 gravity_Position;
    [SerializeField] private float gravity_Diam;
    private GameObject gravity;
    private const float TOLERANCE = 0.1f;
    private const float ACCELERATION_LIMIT = 5;

    private void Start()
    {
        gravity = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        transform.position = m_Start;
        gravity.transform.position = gravity_Position;
        gravity.transform.localScale = new Vector3(gravity_Diam, gravity_Diam, gravity_Diam);
        transform.localScale = new Vector3(m_Diam, m_Diam, m_Diam);
    }

    // Update is called once per frame
    /*void Update()
    {   
        /*gravity.transform.position = gravity_Position;
        gravity.transform.localScale = new Vector3(gravity_Diam, gravity_Diam, gravity_Diam);
        transform.localScale = new Vector3(m_Diam, m_Diam, m_Diam);
        
        Vector3 acceleration = gravity_G * gravity_Weight * (gravity_Position - transform.position) /
                               Mathf.Pow((gravity_Position - transform.position).magnitude, 3);

        if (acceleration.magnitude > ACCELERATION_LIMIT)
        {
            acceleration = acceleration.normalized * ACCELERATION_LIMIT;
        }
        
        Vector3 target = transform.position + m_Speed * Time.deltaTime + acceleration *
            (Mathf.Pow(Time.deltaTime, 2) * 0.5f);
        
        /*Vector3 dir = (target - transform.position).normalized;
        Vector3 delta = dir * (m_Speed.magnitude * Time.deltaTime);
        */
    /*float distance = (gravity_Position - transform.position).magnitude;
     if (distance - gravity_Diam * 0.5f - m_Diam * 0.5f < TOLERANCE)
    {
        m_Speed = dir * 100;
    }
    
    m_Speed += acceleration * Time.deltaTime;
    //transform.Translate(delta);
    transform.position = target;
}*/

    private void FixedUpdate()
    {
        /*gravity.transform.position = gravity_Position;
        gravity.transform.localScale = new Vector3(gravity_Diam, gravity_Diam, gravity_Diam);
        transform.localScale = new Vector3(m_Diam, m_Diam, m_Diam);*/

        Vector3 acceleration = gravity_G * gravity_Weight * (gravity_Position - transform.position) /
                               Mathf.Pow((gravity_Position - transform.position).magnitude, 3);

        if (acceleration.magnitude > ACCELERATION_LIMIT)
        {
            acceleration = acceleration.normalized * ACCELERATION_LIMIT;
        }

        Vector3 target = transform.position + m_Speed * Time.deltaTime + acceleration *
            (Mathf.Pow(Time.deltaTime, 2) * 0.5f);

        /*Vector3 dir = (target - transform.position).normalized;
        Vector3 delta = dir * (m_Speed.magnitude * Time.deltaTime);
        */
        /*float distance = (gravity_Position - transform.position).magnitude;
         if (distance - gravity_Diam * 0.5f - m_Diam * 0.5f < TOLERANCE)
        {
            m_Speed = dir * 100;
        }*/

        m_Speed += acceleration * Time.deltaTime;
        //transform.Translate(delta);
        transform.position = target;
    }
}