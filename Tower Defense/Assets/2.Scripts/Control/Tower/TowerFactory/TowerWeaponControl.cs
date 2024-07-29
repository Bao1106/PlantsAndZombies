using System;
using UnityEngine;

public class TowerWeaponControl
{
    public Action<float> onGetLastAttackTime;
    
    public void AttackTarget(float lastAttackTime, IWeaponModel weaponModel, Transform target, Transform projectile)
    {
        if (Time.time - lastAttackTime >= 1f / weaponModel.GetAttackSpeed())
        {
            weaponModel.Attack(target, projectile);
            lastAttackTime = Time.time;
            onGetLastAttackTime?.Invoke(lastAttackTime);
        }
    }
}
