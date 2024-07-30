using TDEnums;
using UnityEngine;

public class TDCatapultControl : IWeaponModel
{
    private TDFlyweightTowerDataSettings m_Setting;
    private TowerType m_TowerType;

    public void GetType(TowerType type) => m_TowerType = type;
        
    public void Attack(Transform target, Transform spawnProjectile)
    {
            
    }

    public float GetDamage() { return 20f; }
    public float GetAttackSpeed() { return 1f; }
}