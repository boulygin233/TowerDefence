using System;
using UnityEngine;

namespace Turret
{
    public class TurretView : MonoBehaviour
    {
        [SerializeField] private Transform m_ProjectileOrigin;

        public Transform ProjectileOrigin => m_ProjectileOrigin;

        private TurretData m_Data;

        public TurretData Data => m_Data;
        public void AttachData(TurretData turretData)
        {
            m_Data = turretData;
            transform.position = m_Data.Node.Position;
        }
    }
}