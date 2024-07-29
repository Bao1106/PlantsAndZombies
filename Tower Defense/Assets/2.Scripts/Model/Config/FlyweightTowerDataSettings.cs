using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TowerData
{
    public TowerType type;
    public GameObject prefab;
    public int cost;
}
    
[CreateAssetMenu(menuName = "Game Configs/Tower Bullet Config", fileName = "Tower Bullet Config", order = 1)]
public class FlyweightTowerDataSettings : ScriptableObject
{
    [FormerlySerializedAs("bullets")]
    [SerializeField] private List<TowerData> towers;

    private GameObject m_Prefab;
        
    public void SetPrefab(TowerType type)
    {
        m_Prefab = towers.Find(_ => _.type == type)?.prefab;
    }

    public int GetCost(TowerType type)
    {
        return towers.Find(_ => _.type == type).cost;
    }
        
    public BulletsView Create()
    {
        var bullet = Instantiate(m_Prefab).GetComponent<BulletsView>();
        return bullet;
    }
        
    public void OnGet(BulletsView b) => b.gameObject.SetActive(true);
    public void OnRelease(BulletsView b) => b.gameObject.SetActive(false);
    public void OnDestroyObject(BulletsView b) => Destroy(b.gameObject);
}