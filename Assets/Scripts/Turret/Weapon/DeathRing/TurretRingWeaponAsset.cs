using Turret.Weapon.Laser;
using UnityEngine;

namespace Turret.Weapon.DeathRing
{
    [CreateAssetMenu(menuName = "Assets/Turret Ring Weapon Asset", fileName = "Turret Ring Weapon Asset")]
    public class TurretRingWeaponAsset : TurretWeaponAssetBase
    { 
        [SerializeField] public float m_MaxDistance;
        [SerializeField] public float m_Damage;
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretRingWeapon(this, view);
        }
    }
}