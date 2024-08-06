using TDEnums;

public class TDTowerBehaviorModel : IWeaponBehaviorDTO
{
    private static TDTowerBehaviorModel m_api;
    public static TDTowerBehaviorModel api
    {
        get
        {
            return m_api ??= new TDTowerBehaviorModel();
        }
    }

    public TDFlyweightTowerDataSettings settings { get; set; }
    
    public float GetDamage(TowerType type)
    {
        float dmg = type switch
        {
            TowerType.Cannon => 5f,
            TowerType.Catapult => 20f,
            TowerType.MissileG02 => 10f,
            TowerType.MissileG03 => 20f,
            TowerType.Mortar => 15f,
            _ => 0
        };

        return dmg;
    }
    
    public float GetAttackSpeed(TowerType type)
    {
        float speed = type switch
        {
            TowerType.Cannon => 3f,
            TowerType.Catapult => 1f,
            TowerType.MissileG02 => 1f,
            TowerType.MissileG03 => 1f,
            TowerType.Mortar => 2f,
            _ => 0
        };

        return speed;
    }
}
