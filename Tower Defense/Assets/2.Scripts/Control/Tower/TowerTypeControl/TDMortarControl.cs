using TDEnums;
using UnityEngine;

public class TDMortarControl
{
    private TDFlyweightTowerDataSettings m_Setting;
    private TowerType m_TowerType;
        
    public void SetType(TowerType type) => m_TowerType = type;
        
    public void Attack(Transform target, Transform spawnProjectile)
    {
            
    }

    public float GetDamage() { return 15f; }
    public float GetAttackSpeed() { return 2f; }
}