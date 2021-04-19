using Enemy;
using UnityEngine;
using Utils.Pulling;

namespace Turret.Weapon.Projectile.Bullet
{
    [CreateAssetMenu(menuName = "Assets/Bullet Projectile Asset", fileName = "Bullet Projectile Asset")]
    public class BulletProjectileAsset : ProjectileAssetBase
    {
        [SerializeField] private BulletProjectile m_BulletPrefab;
        [SerializeField] public float m_Speed;
        [SerializeField] public float m_Damage;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            BulletProjectile projectile = GameObjectPool.InstantiatePooled(m_BulletPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            projectile.SetAsset(this);
            return projectile;
        }
    }
}














