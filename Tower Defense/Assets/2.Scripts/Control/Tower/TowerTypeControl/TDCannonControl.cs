using TDEnums;
using UnityEngine;

public class TDCannonControl : IWeaponModel
{
    private TDFlyweightTowerDataSettings m_Setting;
    private TowerType m_TowerType;

    public void GetType(TowerType type) => m_TowerType = type;

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
            
        /*for (var i = 0; i < GetAttackSpeed(); i++)
        {

        }*/
    }
        
    public float GetDamage() { return 5f; }
    public float GetAttackSpeed() { return 3f; }
}