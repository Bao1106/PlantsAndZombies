using UnityEngine;

namespace TowerFactory
{
    public class TowerFactory : MonoBehaviour, ITowerFactory
    {
        [SerializeField] private GameObject towerPrefab;
        
        public GameObject CreateTower(Vector3 position)
        {
            return Instantiate(towerPrefab, position, Quaternion.identity);
        }
    }
}