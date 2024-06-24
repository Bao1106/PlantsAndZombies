using Enemy;
using Enums;
using UnityEngine;
using Weapon.Bullets;
using Weapon.Interfaces;

namespace Weapon.Type
{
    public class MissileG03Weapon : IWeapon
    {
        private FlyweightTowerBulletSettings setting;
        private TowerType towerType;

        public void GetType(TowerType type) => towerType = type;
        
        public void Attack(Transform target, Transform spawnProjectile)
        {
            FlyweightBulletFactory.Instance.SetTowerType(towerType);
            
            setting = FlyweightBulletFactory.Instance.Setting;
            setting.SetPrefab(towerType);

            var projectile = FlyweightBulletFactory.Spawn(setting);
            if (projectile != null)
            {
                projectile.transform.position = spawnProjectile.position;
                projectile.Damage = GetDamage();
                
                var rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = (target.position - projectile.transform.position).normalized * 20f;
                }
            }
        }

        public float GetDamage() { return 20f; }
        public float GetAttackSpeed() { return 1f; }
    }
}