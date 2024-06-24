using System;
using Enums;
using Services.DependencyInjection;
using UnityEngine;
using Weapon;
using Weapon.Interfaces;
using Weapon.Range;

namespace TowerFactory
{
    public class TowerFactory : MonoBehaviour, ITowerFactory, IDependencyProvider
    {
        [Provide]
        public ITowerFactory ProviderFactory() => this;
        
        public GameObject CreateTower(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var tower = Instantiate(prefab, position, rotation);
            var weapon = tower.GetComponent<TowerWeapon>();
            var weaponRange = GetWeaponRange(weapon.GetTowerType);
            
            weapon.Initialize(weaponRange);
            return tower;
        }

        private IWeaponRange GetWeaponRange(TowerType type)
        {
            IWeaponRange weaponRange = type switch
            {
                TowerType.Cannon => new HorizontalRange(3),
                TowerType.Catapult => new VerticalRange(2),
                TowerType.MissileG02 => new HorizontalRange(5),
                TowerType.MissileG03 => new AreaRange(6),
                TowerType.Mortar => new HorizontalRange(3),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type,  $"Not expected tower type value: {type}")
            };
            
            return weaponRange;
        }
    }
}