using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grid_Manager;
using TDEnums;
using UnityEngine;
using Object = UnityEngine.Object;

public class TDEnemyPathMainControl
{
    public static TDEnemyPathMainControl api;

    public Action<Vector2Int, Vector2Int> onGetEnemyPos;
    public Action<List<IGridCellModel>> onGetEnemyPath;
    public Action<List<TDEnemyController>> onGetEnemies;
    
    public void InitEnemyPath(IGridManager gridManager, IGridModel gridModel, Vector2Int startPoint, Vector2Int endPoint)
    {
        Vector3 startWorldPos = gridManager.GetNearestGridPosition(new Vector3(startPoint.x * gridManager.cellSize, 0, startPoint.y * gridManager.cellSize));
        Vector3 endWorldPos = gridManager.GetNearestGridPosition(new Vector3(endPoint.x * gridManager.cellSize, 0, endPoint.y * gridManager.cellSize));
        
        startPoint = new Vector2Int(Mathf.RoundToInt(startWorldPos.x / gridManager.cellSize), Mathf.RoundToInt(startWorldPos.z / gridManager.cellSize));
        endPoint = new Vector2Int(Mathf.RoundToInt(endWorldPos.x / gridManager.cellSize), Mathf.RoundToInt(endWorldPos.z / gridManager.cellSize));
        
        gridModel.SetCell(startPoint.x, startPoint.y, new TDGridCellModel(startPoint.x, startPoint.y, CellType.Start));
        gridModel.SetCell(endPoint.x, endPoint.y, new TDGridCellModel(endPoint.x, endPoint.y, CellType.End));
        
        onGetEnemyPos?.Invoke(startPoint, endPoint);
    }

    public void GenerateEnemyPath(IEnemyAIModel enemyAIModel, IGridModel gridModel, 
        Vector2Int startPoint, Vector2Int endPoint, TDEnemyPathView enemyPathView)
    {
        IGridCellModel start = gridModel.GetCell(startPoint.x, startPoint.y);
        IGridCellModel end = gridModel.GetCell(endPoint.x, endPoint.y);

        List<IGridCellModel> lstCell = enemyAIModel
            .CalculatePath(gridModel, start, end, TDConstant.CONFIG_ENEMY_WAYPOINTS);
        
        if (lstCell is { Count: > 0 })
        {
            enemyPathView.VisualizePath(lstCell);
            onGetEnemyPath?.Invoke(lstCell);
        }
        else
        {
            Debug.Log("<color=red>Cannot find path</color>");
        }
    }

    public void SpawnEnemies(TDEnemyController prefab, Transform spawnPos)
    {
        List<TDEnemyController> enemies = new List<TDEnemyController>();
        for (int i = 0; i < TDConstant.CONFIG_ENEMIES_NUMBER; i++)
        {
            TDEnemyController enemy = Object.Instantiate(prefab, spawnPos.position, Quaternion.identity);
            enemies.Add(enemy);
        }
        
        onGetEnemies?.Invoke(enemies);
    }

    public async void SetEnemyPath(List<TDEnemyController> enemies, List<IGridCellModel> paths)
    {
        await Task.Delay(TDConstant.CONFIG_ENEMY_SPAWN_INTERVAL * 1000);
        foreach (TDEnemyController enemy in enemies)
        {
            if (paths is { Count: > 0 })
            {
                if (enemy != null)
                {
                    enemy.SetPath(paths);
                }
                else
                {
                    Debug.Log("<color=red>EnemyController component not found on enemy prefab!</color>");
                }
            }

            await Task.Delay(TDConstant.CONFIG_ENEMY_SPAWN_INTERVAL * 1000);
        }
    }
}
