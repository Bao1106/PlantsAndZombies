using Enemy;
using Enums;
using UnityEngine;

public class MissileG02Control : IWeaponModel
{
    private FlyweightTowerDataSettings m_Setting;
    private TowerType m_TowerType;

    public void GetType(TowerType type) => m_TowerType = type;

    public void Attack(Transform target, Transform spawnProjectile)
    {
        FlyweightBulletFactoryView.Instance.SetTowerType(m_TowerType);
            
        m_Setting = FlyweightBulletFactoryView.Instance.Setting;
        m_Setting.SetPrefab(m_TowerType);

        var projectile = FlyweightBulletFactoryView.Spawn(m_Setting);
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

    public float GetDamage() { return 10f; }
    public float GetAttackSpeed() { return 1f; }
}