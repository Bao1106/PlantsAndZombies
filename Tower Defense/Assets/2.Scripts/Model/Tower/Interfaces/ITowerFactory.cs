﻿using UnityEngine;

namespace TowerFactory
{
    public interface ITowerFactory
    {
        void CreateTower(GameObject prefab, Vector3 position, Quaternion rotation);
    }
}