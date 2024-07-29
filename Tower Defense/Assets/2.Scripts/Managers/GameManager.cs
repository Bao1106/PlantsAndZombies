using Enemy;
using Interfaces.Grid;
using Interfaces.PathFinder;
using UnityEngine;
using Grid = Grid_Manager.Grid;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        /*[SerializeField] private GridManager gridManager;
        [SerializeField] private TowerPlacer.TowerPlacer towerPlacer;
        [SerializeField] private UserInputHandler inputHandler;
        [SerializeField] private TowerFactory.TowerFactory towerFactory;*/
        
        [SerializeField] private EnemyPathManager enemyPathManager;

        private void Awake()
        {
            IGrid grid = new Grid(25, 25);
            IPathFinder pathfinder = new AStarPathControl();
            IEnemyFactoryModel enemyFactoryModel = new EnemyFactoryModelModel(pathfinder);

            enemyPathManager.Initialize(grid, pathfinder, enemyFactoryModel);
        }
    }
}
