using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDEnums;
using Grid_Manager;
using Services.DependencyInjection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class TDEnemyPathManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int startPoint;
        [SerializeField] private Vector2Int endPoint;
        [SerializeField] private List<Vector2Int> waypoints;
        [FormerlySerializedAs("enemy")]
        [SerializeField] private TDEnemyController tdEnemy;
        [SerializeField] private Transform spawnPos;
        
        [SerializeField] private int numberOfEnemies = 5;
        [SerializeField] private float spawnInterval = 5f;
        
        [Inject] private IGridManager m_GridManager;
        [Inject] private TDEnemyPathView m_PathView;
        
        private IGridModel m_GridModel;
        private IEnemyFactoryModel m_AIFactoryModel;
        private IEnemyAIModel m_EnemyAIModel;
        private List<IGridCellModel> m_CurrentPath = new List<IGridCellModel>();
        private readonly List<TDEnemyController> m_EnemiesController = new List<TDEnemyController>();
        private readonly TaskCompletionSource<bool> m_CurrentPathCompletion = new TaskCompletionSource<bool>();
        
        public void Initialize(IGridModel initGridModel, IEnemyFactoryModel initFactoryModel)
        {
            m_GridModel = initGridModel;
            m_AIFactoryModel = initFactoryModel;
        }

        private async void Start()
        {
            await TDInitializeModel.api.createGridCompletion.Task;
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
            
            m_GridModel.SetCell(startPoint.x, startPoint.y, new TDGridCellModel(startPoint.x, startPoint.y, CellType.Start));
            m_GridModel.SetCell(endPoint.x, endPoint.y, new TDGridCellModel(endPoint.x, endPoint.y, CellType.End));

            // Thêm obstacles nếu cần
            // grid.SetCell(x, y, new GridCell(x, y, CellType.Obstacle));
        }
        
        private void GenerateEnemyPath()
        {
            m_EnemyAIModel = m_AIFactoryModel.CreateAI(tdEnemy.AiType);
            
            IGridCellModel start = m_GridModel.GetCell(startPoint.x, startPoint.y);
            IGridCellModel end = m_GridModel.GetCell(endPoint.x, endPoint.y);

            m_CurrentPath = m_EnemyAIModel.CalculatePath(m_GridModel, start, end, waypoints);

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
            var enemyObject = Instantiate(tdEnemy.gameObject, spawnPos.position, Quaternion.identity);
            var enemyController = enemyObject.GetComponent<TDEnemyController>();

            m_EnemiesController.Add(enemyController);
        }

        private IEnumerator SetEnemyPath(List<TDEnemyController> enemies)
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
