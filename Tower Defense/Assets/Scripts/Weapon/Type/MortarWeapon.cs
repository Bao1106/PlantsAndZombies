using Enemy;
using Enums;
using UnityEngine;
using Weapon.Bullets;
using Weapon.Interfaces;

namespace Weapon.Type
{
    public class MortarWeapon : IWeapon
    {
        private FlyweightTowerBulletSettings setting;
        private TowerType towerType;
        
        public void GetType(TowerType type) => towerType = type;
        
        public void Attack(Transform target, Transform spawnProjectile)
        {
            
        }

        public float GetDamage() { return 15f; }
        public float GetAttackSpeed() { return 2f; }
    }
}