using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed;
        private float m_Damage;
        private float m_DamageRadius;
        private bool m_DidHit = false;
        private EnemyData m_HitEnemy = null;
        private EnemyData m_TargetEnemy = null;
        
        public void SetAsset(RocketProjectileAsset rocketProjectileAsset)
        {
            m_Speed = rocketProjectileAsset.m_Speed;
            m_Damage = rocketProjectileAsset.m_Damage;
            m_DamageRadius = rocketProjectileAsset.m_DamageRadius;
        }

        public void TickApproaching()
        {
            transform.Translate((m_TargetEnemy.View.transform.position - transform.position).normalized *
                                (m_Speed * Time.deltaTime), Space.World);
            transform.LookAt(m_TargetEnemy.View.transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_HitEnemy = enemyView.Data;
                }
            }
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            if (m_HitEnemy != null)
            {
                foreach (Node node in Game.Player.Grid.GetNodesInCircle(m_HitEnemy.View.transform.position, m_DamageRadius))
                {
                    foreach (EnemyData enemyData in node.m_EnemyDatas)
                    {
                        enemyData.GetDamage(m_Damage);
                    }
                }
            }
            Destroy(gameObject);
        }

        public void SetTargetEnemy(EnemyData enemyData)
        {
            m_TargetEnemy = enemyData;
        }
    }
}