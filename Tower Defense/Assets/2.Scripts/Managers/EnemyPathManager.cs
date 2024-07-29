using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
using Enums;
using Grid_Manager;
using Interfaces.Grid;
using Interfaces.PathFinder;
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
        
        [Inject] private IGridManager m_GridManager;
        [Inject] private EnemyPathView m_PathView;
        
        private IGrid m_Grid;
        private IPathFinder m_PathFinder;
        private IEnemyFactoryModel m_AIFactoryModel;
        private IEnemyAIModel m_EnemyAIModel;
        private List<IGridCell> m_CurrentPath = new List<IGridCell>();
        private readonly List<EnemyController> m_EnemiesController = new List<EnemyController>();
        private readonly TaskCompletionSource<bool> m_CurrentPathCompletion = new TaskCompletionSource<bool>();
        
        public void Initialize(IGrid initGrid, IPathFinder initPathFinder, IEnemyFactoryModel initFactoryModel)
        {
            m_Grid = initGrid;
            m_PathFinder = initPathFinder;
            m_AIFactoryModel = initFactoryModel;
        }

        private async void Start()
        {
            await InitializeModel.api.createGridCompletion.Task;
            SetupGrid();
            GenerateEnemyPath();
            SpawnEnemies();
            StartCoroutine(SetEnemyPath(m_EnemiesController));
        }
        
        private void SetupGrid()
        {
            Vector3 startWorldPos = m_GridManager.GetNearestGridPosition(new Vector3(startPoint.x * m_GridManager.cellSize, 0, startPoint.y * m_GridManager.cellSize));
            Vector3 endWorldPos = m_GridManager.GetNearestGridPosition(new Vector3(endPoint.x * m_GridManager.cellSize, 0, endPoint.y * m_GridManager.cellSize));
            
            startPoint = new Vector2Int(Mathf.RoundToInt(startWorldPos.x / m_GridManager.cellSize), Mathf.RoundToInt(startWorldPos.z / m_GridManager.cellSize));
            endPoint = new Vector2Int(Mathf.RoundToInt(endWorldPos.x / m_GridManager.cellSize), Mathf.RoundToInt(endWorldPos.z / m_GridManager.cellSize));
            
            m_Grid.SetCell(startPoint.x, startPoint.y, new GridCell(startPoint.x, startPoint.y, CellType.Start));
            m_Grid.SetCell(endPoint.x, endPoint.y, new GridCell(endPoint.x, endPoint.y, CellType.End));

            // Thêm obstacles nếu cần
            // grid.SetCell(x, y, new GridCell(x, y, CellType.Obstacle));
        }
        
        private void GenerateEnemyPath()
        {
            m_EnemyAIModel = m_AIFactoryModel.CreateAI(enemy.AiType);
            
            IGridCell start = m_Grid.GetCell(startPoint.x, startPoint.y);
            IGridCell end = m_Grid.GetCell(endPoint.x, endPoint.y);

            m_CurrentPath = m_EnemyAIModel.CalculatePath(m_Grid, start, end, waypoints);

            m_CurrentPathCompletion.SetResult(true);
            
            if (m_CurrentPath is { Count: > 0 })
            {
                m_PathView.VisualizePath(m_CurrentPath);
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

            m_EnemiesController.Add(enemyController);
        }

        private IEnumerator SetEnemyPath(List<EnemyController> enemies)
        {
            yield return new WaitForSeconds(5f);
            
            foreach (var enemyController in enemies)
            {
                if (m_CurrentPath is { Count: > 0 })
                {
                    if (enemyController != null)
                    {
                        enemyController.SetPath(m_CurrentPath);
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
