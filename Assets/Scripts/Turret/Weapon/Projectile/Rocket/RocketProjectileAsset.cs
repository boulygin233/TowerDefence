using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAsset : ProjectileAssetBase
    {
        [SerializeField] private RocketProjectile m_RocketPrefab;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile prefab = Instantiate(m_RocketPrefab, origin, 
                Quaternion.LookRotation(originForward, Vector3.up));
            prefab.SetTargetEnemy(enemyData);
            return prefab;
        }
    }
}