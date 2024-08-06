using TDEnums;
using UnityEngine;

public class TDMissileG03Control
{
    private TDFlyweightTowerDataSettings m_Setting;
    private TowerType m_TowerType;

    public void SetType(TowerType type) => m_TowerType = type;
        
    public void Attack(Transform target, Transform spawnProjectile)
    {
        TDFlyweightBulletFactoryModel.api.SetTowerType(m_TowerType);
            
        m_Setting = TDFlyweightBulletFactoryModel.api.Setting;
        m_Setting.SetPrefab(m_TowerType);

        var projectile = TDFlyweightBulletFactoryModel.Spawn(m_Setting);
        if (projectile != null)
        {
            projectile.transform.position = spawnProjectile.position;
            projectile.Damage = GetDamage();
                
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (target.position - projectile.transform.position).normalized * 20f;
            }
        }
    }

    public float GetDamage() { return 20f; }
    public float GetAttackSpeed() { return 1f; }
}