﻿using Enemy;
using UnityEngine;
using Utils.Pulling;

namespace Turret.Weapon.Projectile.Bullet
{
    public class BulletProjectile : PooledMonoBehaviour, IProjectile
    {
        private float m_Speed;
        private float m_Damage;
        private bool m_DidHit = false;
        private EnemyData m_HitEnemy = null;

        public override void AwakePooled()
        {
            m_DidHit = false;
            m_HitEnemy = null;
        }

        public void SetAsset(BulletProjectileAsset bulletProjectileAsset)
        {
            m_Speed = bulletProjectileAsset.m_Speed;
            m_Damage = bulletProjectileAsset.m_Damage;
        }
        public void TickApproaching()
        {
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
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
                Debug.Log("Hit!");
                m_HitEnemy.GetDamage(m_Damage);
            }
            GameObjectPool.ReturnObjectToPool(this);
        }
    }
}














