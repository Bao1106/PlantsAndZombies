using Enemy;
using Enums;
using UnityEngine;
using Weapon.Bullets;
using Weapon.Interfaces;

namespace Weapon.Type
{
    public class CatapultWeapon : IWeapon
    {
        private FlyweightTowerBulletSettings setting;
        private TowerType towerType;

        public void GetType(TowerType type) => towerType = type;
        
        public void Attack(Transform target, Transform spawnProjectile)
        {
            
        }

        public float GetDamage() { return 20f; }
        public float GetAttackSpeed() { return 1f; }
    }
}