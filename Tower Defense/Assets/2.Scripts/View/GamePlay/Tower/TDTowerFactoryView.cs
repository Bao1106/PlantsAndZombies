using System;
using TDEnums;
using UnityEngine;

public class TDTowerFactoryView : MonoBehaviour
    {
        private void Start()
        {
            RegistryTowerFactoryEvents();
        }

        private void RegistryTowerFactoryEvents()
        {
            TDTowerFactoryControl.api.onCreateTowerSuccess += OnCreateTowerSuccess;
        }

        private void OnDestroy()
        {
            TDTowerFactoryControl.api.onCreateTowerSuccess -= OnCreateTowerSuccess;
        }

        private void OnCreateTowerSuccess(GameObject tower)
        {
            TDTowerWeaponView tdTowerWeaponView = tower.GetComponent<TDTowerWeaponView>();
            IWeaponRangeModel weaponRangeModel = GetWeaponRange(tdTowerWeaponView.GetTowerType);
            IWeaponModel weaponModel = GetWeapon(tdTowerWeaponView.GetTowerType);
            
            tdTowerWeaponView.Init(weaponRangeModel, weaponModel);
        }

        private IWeaponRangeModel GetWeaponRange(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new TDHorizontalRangeModel(3),
                TowerType.Catapult => new TDVerticalRangeModel(2),
                TowerType.MissileG02 => new TDHorizontalRangeModel(5),
                TowerType.MissileG03 => new TDAreaRangeModel(6),
                TowerType.Mortar => new TDHorizontalRangeModel(3),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type,  $"Not expected tower type value: {type}")
            };
        }
        
        private IWeaponModel GetWeapon(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new TDCannonControl(),
                TowerType.Catapult => new TDCatapultControl(),
                TowerType.MissileG02 => new TDMissileG02Control(),
                TowerType.MissileG03 => new TDMissileG03Control(),
                TowerType.Mortar => new TDMortarControl(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"Not expected tower type value: {type}")
            };
        }
    }