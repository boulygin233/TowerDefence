using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using Turret.Weapon.Laser;
using UnityEngine;

namespace Turret.Weapon.DeathRing
{
    public class TurretRingWeapon : ITurretWeapon
    {
        private TurretView m_View;
        private TurretRingWeaponAsset m_Asset;
        private List<Node> m_NodesInCircle;
        private float m_MaxDistance;
        private float m_Damage;
        public TurretRingWeapon(TurretRingWeaponAsset asset, TurretView view)
        {
            m_View = view;
            m_Asset = asset;
            m_MaxDistance = m_Asset.m_MaxDistance;
            m_Damage = m_Asset.m_Damage;
            m_NodesInCircle = Game.Player.Grid.GetNodesInCircle(view.transform.position, m_MaxDistance);
        }


        public void TickShoot()
        {
            foreach (Node node in m_NodesInCircle)
            {
                foreach (EnemyData enemyData in node.m_EnemyDatas)
                {
                    enemyData.GetDamage(m_Damage);
                }
            }
        }
    }
}