using System;
using Enums;
using UnityEngine;

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
            TowerWeaponView towerWeaponView = tower.GetComponent<TowerWeaponView>();
            IWeaponRangeModel weaponRangeModel = GetWeaponRange(towerWeaponView.GetTowerType);
            IWeaponModel weaponModel = GetWeapon(towerWeaponView.GetTowerType);
            
            towerWeaponView.Init(weaponRangeModel, weaponModel);
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
                TowerType.Cannon => new CannonControl(),
                TowerType.Catapult => new CatapultControl(),
                TowerType.MissileG02 => new MissileG02Control(),
                TowerType.MissileG03 => new MissileG03Control(),
                TowerType.Mortar => new MortarControl(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"Not expected tower type value: {type}")
            };
        }
    }