using UnityEngine;

namespace TowerFactory
{
    public interface ITowerFactory
    {
        GameObject CreateTower(Vector3 position);
    }
}