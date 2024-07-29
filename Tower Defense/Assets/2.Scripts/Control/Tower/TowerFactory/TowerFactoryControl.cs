using System;
using TowerFactory;
using UnityEngine;
using Object = UnityEngine.Object;

public class TowerFactoryControl : ITowerFactory
{
    public static TowerFactoryControl api;

    public Action<GameObject> onCreateTowerSuccess;
    
    public void CreateTower(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject tower = Object.Instantiate(prefab, position, rotation);
        onCreateTowerSuccess?.Invoke(tower);
    }
}
