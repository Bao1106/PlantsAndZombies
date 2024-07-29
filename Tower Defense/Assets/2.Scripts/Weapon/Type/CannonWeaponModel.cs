using System.Collections;
using Enemy;
using Enums;
using Services.Utils;
using UnityEngine;
using Weapon.Bullets;
using Weapon.Interfaces;

namespace Weapon.Type
{
    public class CannonWeaponModel : IWeaponModel
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
            
            /*for (var i = 0; i < GetAttackSpeed(); i++)
            {
                
            }*/
        }
        
        public float GetDamage() { return 5f; }
        public float GetAttackSpeed() { return 3f; }
    }
}