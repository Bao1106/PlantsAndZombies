using System;
using TDEnums;
using UnityEngine;

public class TDTowerWeaponControl
{
    public static TDTowerWeaponControl api;
    
    public Action<string, float> onGetLastAttackTime;
    
    public void AttackTarget(float lastAttackTime, Transform target, Transform projectile, string key, TowerType type)
    {
        if (Time.time - lastAttackTime >= 1f / TDTowerBehaviorModel.api.GetAttackSpeed(type))
        {
            TDTowerBehaviorControl.api.Attack(target, projectile, type);
            lastAttackTime = Time.time;
            onGetLastAttackTime?.Invoke(key, lastAttackTime);
        }
    }
}
