using Services.DependencyInjection;
using UnityEngine;

namespace TowerFactory
{
    public class TowerFactory : MonoBehaviour, ITowerFactory, IDependencyProvider
    {
        [Provide]
        public ITowerFactory ProviderFactory() => this;
        
        public GameObject CreateTower(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Instantiate(prefab, position, rotation);
        }
    }
}