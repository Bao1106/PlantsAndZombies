using Enemy;
using Enums;
using UnityEngine;

namespace Weapon.Interfaces
{
    public interface IWeapon
    {
        void GetType(TowerType type);
        void Attack(Transform target, Transform spawnProjectile);
        float GetDamage();
        float GetAttackSpeed();
    }
}