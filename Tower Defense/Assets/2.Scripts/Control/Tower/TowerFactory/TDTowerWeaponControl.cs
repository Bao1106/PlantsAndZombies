using System;
using UnityEngine;

public class TDTowerWeaponControl
{
    public static TDTowerWeaponControl api;
    
    public Action<string, float> onGetLastAttackTime;
    
    public void AttackTarget(float lastAttackTime, IWeaponModel weaponModel, Transform target, Transform projectile, string key)
    {
        if (Time.time - lastAttackTime >= 1f / weaponModel.GetAttackSpeed())
        {
            weaponModel.Attack(target, projectile);
            lastAttackTime = Time.time;
            onGetLastAttackTime?.Invoke(key, lastAttackTime);
        }
    }
}
