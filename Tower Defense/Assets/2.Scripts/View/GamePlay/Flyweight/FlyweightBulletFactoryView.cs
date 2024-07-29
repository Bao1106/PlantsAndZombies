using System.Collections.Generic;
using Enums;
using Services.Utils;
using UnityEngine.Pool;

public class FlyweightBulletFactoryView : Singleton<FlyweightBulletFactoryView>
{
    private FlyweightTowerDataSettings m_Setting;
    private readonly bool m_CollectionCheck = true;
    private readonly int m_MaxCapacity = 100;
    private int m_DefaultCapacity;
        
    private readonly Dictionary<TowerType, IObjectPool<BulletsView>> m_Pools = new Dictionary<TowerType, IObjectPool<BulletsView>>();
    private TowerType m_TowerType;

    public FlyweightTowerDataSettings Setting
    {
        get
        {
            if (m_Setting != null)
                return m_Setting;
                    
            return m_Setting = ResourceObject.GetResource<FlyweightTowerDataSettings>(DTConstant.CONFIG_TOWER);
        }
    }
        
    public void SetTowerType(TowerType type) => m_TowerType = type;
        
    public static BulletsView Spawn(FlyweightTowerDataSettings s)
        => Instance.GetPoolFor(s).Get();

    public static void ReturnToPool(BulletsView s)
        => Instance.GetPoolFor(Instance.m_Setting)?.Release(s);
        
    private IObjectPool<BulletsView> GetPoolFor(FlyweightTowerDataSettings settings)
    {
        if (m_Pools.TryGetValue(m_TowerType, out var pool)) 
            return pool;

        pool = new ObjectPool<BulletsView>(
            settings.Create,
            settings.OnGet,
            settings.OnRelease,
            settings.OnDestroyObject,
            m_CollectionCheck,
            m_DefaultCapacity,
            m_MaxCapacity);
        m_Pools.Add(m_TowerType, pool);
            
        return pool;
    }
}