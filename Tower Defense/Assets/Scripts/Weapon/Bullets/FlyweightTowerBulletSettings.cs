using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Weapon.Bullets
{
    [Serializable]
    public class TowerBullets
    {
        public TowerType type;
        public GameObject prefab;
    }
    
    [CreateAssetMenu(menuName = "Game Configs/Tower Bullet Config", fileName = "Tower Bullet Config", order = 1)]
    public class FlyweightTowerBulletSettings : ScriptableObject
    {
        [SerializeField] private List<TowerBullets> bullets;

        private GameObject prefab;
        
        public void SetPrefab(TowerType type)
        {
            prefab = bullets.Find(_ => _.type == type)?.prefab;
        }

        public Bullets Create()
        {
            var bullet = Instantiate(prefab).GetComponent<Bullets>();
            return bullet;
        }
        
        public void OnGet(Bullets b) => b.gameObject.SetActive(true);
        public void OnRelease(Bullets b) => b.gameObject.SetActive(false);
        public void OnDestroyObject(Bullets b) => Destroy(b.gameObject);
    }
}