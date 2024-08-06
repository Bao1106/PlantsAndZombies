using TDEnums;
using UnityEngine;

public interface IWeaponBehaviorDTO
{
    float GetDamage(TowerType type);
    float GetAttackSpeed(TowerType type);
}