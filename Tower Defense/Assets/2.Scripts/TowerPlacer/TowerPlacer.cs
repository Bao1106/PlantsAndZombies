using Grid_Manager;
using Services.DependencyInjection;
using TowerFactory;
using UnityEngine;

namespace TowerPlacer
{
    public class TowerPlacer : MonoBehaviour, ITowerPlacer, IDependencyProvider
    {
        //[SerializeField] private GameObject towerPrefab;
        [Inject] private IGridManager m_GridManager;
        [Inject] private ITowerFactory m_TowerFactory;
        [Inject] private TowerMainView m_TowerMainView;
        
        [Provide]
        public ITowerPlacer ProviderTowerPlacer()
        {
            return this;
        }
        
        public void PlaceTower(Vector3 position)
        {
            // Kiểm tra xem vị trí có hợp lệ không
            if (m_GridManager.IsValidPlacement(position))
            {
                var nearestPosition = m_GridManager.GetNearestGridPosition(position);
                var rotation = m_TowerMainView.currentTower.transform.rotation;
                m_TowerFactory.CreateTower(m_TowerMainView.currentTower, nearestPosition, rotation);
                Destroy(m_TowerMainView.currentTower);
                m_TowerMainView.currentTower = null;
                
                m_GridManager.SetOccupiedCell(nearestPosition);
            }
        }
    }
}