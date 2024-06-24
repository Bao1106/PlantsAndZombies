using System.Collections.Generic;
using Enums;
using Services.Utils;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapon.Bullets
{
    public class FlyweightBulletFactory : Singleton<FlyweightBulletFactory>
    {
        [SerializeField] private FlyweightTowerBulletSettings setting;
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private int maxCapacity = 100;
        [SerializeField] private int defaultCapacity;
        
        private readonly Dictionary<TowerType, IObjectPool<Bullets>> pools = new();
        private TowerType towerType;

        public FlyweightTowerBulletSettings Setting => setting;
        
        public void SetTowerType(TowerType type) => towerType = type;
        
        public static Bullets Spawn(FlyweightTowerBulletSettings s)
            => Instance.GetPoolFor(s).Get();

        public static void ReturnToPool(Bullets s)
            => Instance.GetPoolFor(Instance.setting)?.Release(s);
        
        private IObjectPool<Bullets> GetPoolFor(FlyweightTowerBulletSettings settings)
        {
            if (pools.TryGetValue(towerType, out var pool)) 
                return pool;

            pool = new ObjectPool<Bullets>(
                settings.Create,
                settings.OnGet,
                settings.OnRelease,
                settings.OnDestroyObject,
                collectionCheck,
                defaultCapacity,
                maxCapacity);
            pools.Add(towerType, pool);
            
            return pool;
        }
    }
}