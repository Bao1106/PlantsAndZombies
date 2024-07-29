﻿using Enums;
using UnityEngine;

public class MortarControl : IWeaponModel
{
    private FlyweightTowerDataSettings m_Setting;
    private TowerType m_TowerType;
        
    public void GetType(TowerType type) => m_TowerType = type;
        
    public void Attack(Transform target, Transform spawnProjectile)
    {
            
    }

    public float GetDamage() { return 15f; }
    public float GetAttackSpeed() { return 2f; }
}