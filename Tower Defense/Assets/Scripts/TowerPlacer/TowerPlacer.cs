using Grid_Manager;
using Services.DependencyInjection;
using TowerFactory;
using UnityEngine;

namespace TowerPlacer
{
    public class TowerPlacer : MonoBehaviour, ITowerPlacer, IDependencyProvider
    {
        //[SerializeField] private GameObject towerPrefab;
        [Inject] private IGridManager gridManager;
        [Inject] private TowerSelector towerSelector;
        
        [Provide]
        public ITowerPlacer ProviderTowerPlacer()
        {
            return this;
        }
        
        public void PlaceTower(Vector3 position)
        {
            // Kiểm tra xem vị trí có hợp lệ không
            if (gridManager.IsValidPlacement(position))
            {
                var nearestPosition = gridManager.GetNearestGridPosition(position);
                Instantiate(towerSelector.currentTower, nearestPosition, Quaternion.identity);
                DestroyImmediate(towerSelector.currentTower);
                towerSelector.currentTower = null;
                
                gridManager.SetOccupiedCell(nearestPosition);
            }
        }
    }
}