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
            string key = tower.gameObject.name;
            TDTowerWeaponView tdTowerWeaponView = tower.GetComponent<TDTowerWeaponView>();
            TDTowerBehaviorControl.api.SetupSubControl(tdTowerWeaponView.towerType);
            IWeaponRangeDTO weaponRangeDTO = GetWeaponRange(tdTowerWeaponView.towerType);
            
            tdTowerWeaponView.Init(weaponRangeDTO, key);
        }

        private IWeaponRangeDTO GetWeaponRange(TowerType type)
        {
            return type switch
            {
                TowerType.Cannon => new TDHorizontalRangeDTO(3),
                TowerType.Catapult => new TDVerticalRangeDTO(2),
                TowerType.MissileG02 => new TDHorizontalRangeDTO(5),
                TowerType.MissileG03 => new TDAreaRangeDTO(6),
                TowerType.Mortar => new TDHorizontalRangeDTO(3),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type,  $"Not expected tower type value: {type}")
            };
        }
    }