using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
using Enemy.Interfaces;
using Enums;
using Grid_Manager;
using Interfaces.Grid;
using Interfaces.PathFinder;
using PathFinder;
using Services.DependencyInjection;
using UnityEngine;

namespace Managers
{
    public class EnemyPathManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int startPoint;
        [SerializeField] private Vector2Int endPoint;
        [SerializeField] private List<Vector2Int> waypoints;
        [SerializeField] private EnemyController enemy;
        [SerializeField] private Transform spawnPos;
        
        [SerializeField] private int numberOfEnemies = 5;
        [SerializeField] private float spawnInterval = 5f;
        
        [Inject] private IGridManager gridManager;
        [Inject] private EnemyPathVisualizer pathVisualizer;
        
        private IGrid grid;
        private IPathFinder pathFinder;
        private IEnemyFactory aiFactory;
        private IEnemyAI enemyAI;
        private List<IGridCell> currentPath = new();
        private List<EnemyController> enemiesController = new();

        private readonly TaskCompletionSource<bool> currentPathCompletion = new();
        
        public void Initialize(IGrid initGrid, IPathFinder initPathFinder, IEnemyFactory initFactory)
        {
            grid = initGrid;
            pathFinder = initPathFinder;
            aiFactory = initFactory;
        }

        private async void Start()
        {
            await Initializer.Instance.CreateGridCompletion.Task;
            SetupGrid();
            GenerateEnemyPath();
            SpawnEnemies();
            StartCoroutine(SetEnemyPath(enemiesController));
        }
        
        private void SetupGrid()
        {
            var startWorldPos = gridManager.GetNearestGridPosition(new Vector3(startPoint.x * gridManager.CellSize, 0, startPoint.y * gridManager.CellSize));
            var endWorldPos = gridManager.GetNearestGridPosition(new Vector3(endPoint.x * gridManager.CellSize, 0, endPoint.y * gridManager.CellSize));
            
            startPoint = new Vector2Int(Mathf.RoundToInt(startWorldPos.x / gridManager.CellSize), Mathf.RoundToInt(startWorldPos.z / gridManager.CellSize));
            endPoint = new Vector2Int(Mathf.RoundToInt(endWorldPos.x / gridManager.CellSize), Mathf.RoundToInt(endWorldPos.z / gridManager.CellSize));
            
            grid.SetCell(startPoint.x, startPoint.y, new GridCell(startPoint.x, startPoint.y, CellType.Start));
            grid.SetCell(endPoint.x, endPoint.y, new GridCell(endPoint.x, endPoint.y, CellType.End));

            // Thêm obstacles nếu cần
            // grid.SetCell(x, y, new GridCell(x, y, CellType.Obstacle));
        }
        
        private void GenerateEnemyPath()
        {
            enemyAI = aiFactory.CreateAI(enemy.AiType);
            
            var start = grid.GetCell(startPoint.x, startPoint.y);
            var end = grid.GetCell(endPoint.x, endPoint.y);

            currentPath = enemyAI.CalculatePath(grid, start, end, waypoints);

            currentPathCompletion.SetResult(true);
            
            if (currentPath is { Count: > 0 })
            {
                pathVisualizer.VisualizePath(currentPath);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy đường đi!");
            }
        }
        
        private void SpawnEnemies()
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var enemyObject = Instantiate(enemy.gameObject, spawnPos.position, Quaternion.identity);
            var enemyController = enemyObject.GetComponent<EnemyController>();

            enemiesController.Add(enemyController);
        }

        private IEnumerator SetEnemyPath(List<EnemyController> enemies)
        {
            yield return new WaitForSeconds(5f);
            
            foreach (var enemyController in enemies)
            {
                if (currentPath is { Count: > 0 })
                {
                    if (enemyController != null)
                    {
                        enemyController.SetPath(currentPath);
                    }
                    else
                    {
                        Debug.LogError("EnemyController component not found on enemy prefab!");
                    }
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
