using UnityEngine;

namespace Turret.Weapon.Laser
{
    [CreateAssetMenu(menuName = "Assets/Turret Laser Weapon Asset", fileName = "Turret Laser Weapon Asset")]
    public class TurretLaserWeaponAsset : TurretWeaponAssetBase
    {
        public LineRenderer LineRendererPrefab;
        public float m_MaxDistance;
    
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretLaserWeapon(this, view);
        }
    }
}