using Enums;
using UnityEngine;
using Weapon.Interfaces;

public class TowerModel
{
    public TowerType type { get; private set; }
    public int cost { get; private set; }
    public IWeaponRangeModel weaponRangeModel { get; private set; }
    public IWeaponModel weaponModel { get; private set; }
    
    public TowerModel(TowerType towerType, int towerCost, IWeaponRangeModel weaponRange, IWeaponModel weapon)
    {
        type = towerType;
        cost = towerCost;
        weaponRangeModel = weaponRange;
        weaponModel = weapon;
    }
}
