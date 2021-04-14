using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Laser
{
    public class TurretLaserWeapon : ITurretWeapon

    {
        private LineRenderer m_LineRenderer;
        private TurretView m_View;
        private TurretLaserWeaponAsset m_Asset;
        private List<Node> m_NodesInCircle;
        private float m_MaxDistance;
        private float m_Damage = 3f;
        [CanBeNull] private EnemyData m_ClosestEnemyData;

        public TurretLaserWeapon(TurretLaserWeaponAsset asset, TurretView view)
        {
            m_View = view;
            m_Asset = asset;
            m_MaxDistance = m_Asset.m_MaxDistance;
            m_NodesInCircle = Game.Player.Grid.GetNodesInCircle(view.transform.position, m_MaxDistance);
            m_LineRenderer = Object.Instantiate(asset.LineRendererPrefab, m_View.ProjectileOrigin.transform);
        }


        public void TickShoot()
        {
            TickWeapon();
            TickTower();
        }

        private void TickWeapon()
        {
            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_NodesInCircle);
            if (m_ClosestEnemyData == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
                return;
            }
            m_LineRenderer.gameObject.SetActive(true);
            var position = m_View.ProjectileOrigin.position;
            m_LineRenderer.transform.position = position;
            Transform transform;
            (transform = m_LineRenderer.transform).LookAt(m_ClosestEnemyData.View.transform);
            
            Vector3 laserScale = transform.localScale;
            laserScale.z = (m_ClosestEnemyData.View.transform.position - position).magnitude;
            transform.localScale = laserScale;

            TickTower();
            m_ClosestEnemyData.GetDamage(m_Damage * Time.deltaTime);
        }
        
        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }
    }
}