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
    public class TowerFactoryView : MonoBehaviour
    {
        private void Start()
        {
            RegistryTowerFactoryEvents();
        }

        private void RegistryTowerFactoryEvents()
        {
            TowerFactoryControl.api.onCreateTowerSuccess += OnCreateTowerSuccess;
        }

        private void OnDestroy()
        {
            TowerFactoryControl.api.onCreateTowerSuccess -= OnCreateTowerSuccess;
        }

        private void OnCreateTowerSuccess(GameObject tower)
        {
            TowerWeapon towerWeapon = tower.GetComponent<TowerWeapon>();
            IWeaponRangeModel weaponRangeModel = GetWeaponRange(towerWeapon.GetTowerType);
            IWeaponModel weaponModel = GetWeapon(towerWeapon.GetTowerType);
            
            towerWeapon.Init(weaponRangeModel, weaponModel);
        }

        private IWeaponRangeModel GetWeaponRange(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new HorizontalRangeModel(3),
                TowerType.Catapult => new VerticalRangeModel(2),
                TowerType.MissileG02 => new HorizontalRangeModel(5),
                TowerType.MissileG03 => new AreaRangeModel(6),
                TowerType.Mortar => new HorizontalRangeModel(3),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type,  $"Not expected tower type value: {type}")
            };
        }
        
        private IWeaponModel GetWeapon(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new CatapultWeaponModel(),
                TowerType.Catapult => new CatapultWeaponModel(),
                TowerType.MissileG02 => new MissileG02WeaponModel(),
                TowerType.MissileG03 => new MissileG03WeaponModel(),
                TowerType.Mortar => new MortarWeaponModel(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"Not expected tower type value: {type}")
            };
        }
    }
}