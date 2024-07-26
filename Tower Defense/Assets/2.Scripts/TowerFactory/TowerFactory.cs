using System;
using Enums;
using Services.DependencyInjection;
using UnityEngine;
using Weapon;
using Weapon.Interfaces;
using Weapon.Range;
using Weapon.Type;

namespace TowerFactory
{
    public class TowerFactory : MonoBehaviour, ITowerFactory, IDependencyProvider
    {
        [Provide]
        public ITowerFactory ProviderFactory() => this;
        
        public GameObject CreateTower(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var tower = Instantiate(prefab, position, rotation);
            var towerWeapon = tower.GetComponent<TowerWeapon>();
            var weaponRange = GetWeaponRange(towerWeapon.GetTowerType);
            var weapon = GetWeapon(towerWeapon.GetTowerType);
            
            towerWeapon.Initialize(weaponRange, weapon);
            return tower;
        }

        private IWeaponRange GetWeaponRange(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new HorizontalRange(3),
                TowerType.Catapult => new VerticalRange(2),
                TowerType.MissileG02 => new HorizontalRange(5),
                TowerType.MissileG03 => new AreaRange(6),
                TowerType.Mortar => new HorizontalRange(3),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type,  $"Not expected tower type value: {type}")
            };
        }
        
        private IWeapon GetWeapon(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new CannonWeapon(),
                TowerType.Catapult => new CatapultWeapon(),
                TowerType.MissileG02 => new MissileG02Weapon(),
                TowerType.MissileG03 => new MissileG03Weapon(),
                TowerType.Mortar => new MortarWeapon(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"Not expected tower type value: {type}")
            };
        }
    }
}