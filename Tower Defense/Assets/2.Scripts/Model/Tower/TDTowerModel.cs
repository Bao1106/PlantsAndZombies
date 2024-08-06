using TDEnums;

public class TDTowerModel
{
    public TowerType type { get; private set; }
    public int cost { get; private set; }
    public IWeaponRangeDTO WeaponRangeDTO { get; private set; }
    public IWeaponBehaviorDTO WeaponBehaviorDTO { get; private set; }
    
    public TDTowerModel(TowerType towerType, int towerCost, IWeaponRangeDTO weaponRange, IWeaponBehaviorDTO weaponBehavior)
    {
        type = towerType;
        cost = towerCost;
        WeaponRangeDTO = weaponRange;
        WeaponBehaviorDTO = weaponBehavior;
    }
}
