﻿using System.Collections.Generic;
using TDEnums;
using Services.Utils;
using UnityEngine.Pool;

public class TDFlyweightBulletFactoryView : Singleton<TDFlyweightBulletFactoryView>
{
    private TDFlyweightTowerDataSettings m_Setting;
    private readonly bool m_CollectionCheck = true;
    private readonly int m_MaxCapacity = 100;
    private int m_DefaultCapacity;
        
    private readonly Dictionary<TowerType, IObjectPool<TDBulletsView>> m_Pools = new Dictionary<TowerType, IObjectPool<TDBulletsView>>();
    private TowerType m_TowerType;

    public TDFlyweightTowerDataSettings Setting
    {
        get
        {
            if (m_Setting != null)
                return m_Setting;
                    
            return m_Setting = ResourceObject.GetResource<TDFlyweightTowerDataSettings>(TDConstant.CONFIG_TOWER);
        }
    }
        
    public void SetTowerType(TowerType type) => m_TowerType = type;
        
    public static TDBulletsView Spawn(TDFlyweightTowerDataSettings s)
        => Instance.GetPoolFor(s).Get();

    public static void ReturnToPool(TDBulletsView s)
        => Instance.GetPoolFor(Instance.m_Setting)?.Release(s);
        
    private IObjectPool<TDBulletsView> GetPoolFor(TDFlyweightTowerDataSettings settings)
    {
        if (m_Pools.TryGetValue(m_TowerType, out var pool)) 
            return pool;

        pool = new ObjectPool<TDBulletsView>(
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