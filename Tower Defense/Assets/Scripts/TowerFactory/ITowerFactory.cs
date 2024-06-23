using UnityEngine;

namespace TowerFactory
{
    public interface ITowerFactory
    {
        GameObject CreateTower(GameObject prefab, Vector3 position);
    }
}