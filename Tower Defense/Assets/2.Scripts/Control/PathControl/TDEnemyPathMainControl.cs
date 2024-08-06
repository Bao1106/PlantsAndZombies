using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDEnums;
using UnityEngine;
using Object = UnityEngine.Object;

public class TDEnemyPathMainControl
{
    public static TDEnemyPathMainControl api;

    public Action<Vector2Int, Vector2Int> onGetEnemyPos;
    public Action<List<IGridCellModel>> onGetEnemyPath;
    public Action<List<TDEnemyView>> onGetEnemies;
    
    public void InitEnemyPath(IGridModel gridModel, Vector2Int startPoint, Vector2Int endPoint)
    {
        Vector3 startWorldPos = TDGridMainModel.api.GetNearestGridPosition(new Vector3(startPoint.x * TDGridMainModel.api.cellSize, 0, startPoint.y * TDGridMainModel.api.cellSize));
        Vector3 endWorldPos = TDGridMainModel.api.GetNearestGridPosition(new Vector3(endPoint.x * TDGridMainModel.api.cellSize, 0, endPoint.y * TDGridMainModel.api.cellSize));
        
        startPoint = new Vector2Int(Mathf.RoundToInt(startWorldPos.x / TDGridMainModel.api.cellSize), Mathf.RoundToInt(startWorldPos.z / TDGridMainModel.api.cellSize));
        endPoint = new Vector2Int(Mathf.RoundToInt(endWorldPos.x / TDGridMainModel.api.cellSize), Mathf.RoundToInt(endWorldPos.z / TDGridMainModel.api.cellSize));
        
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

    public void SpawnEnemies(TDEnemyView prefab, Transform spawnPos)
    {
        List<TDEnemyView> enemies = new List<TDEnemyView>();
        for (int i = 0; i < TDConstant.CONFIG_ENEMIES_NUMBER; i++)
        {
            TDEnemyView enemy = Object.Instantiate(prefab, spawnPos.position, Quaternion.identity);
            string key = $"{i}-{enemy.gameObject.name}";
            enemy.Initialize(key);
            enemies.Add(enemy);
        }
        
        onGetEnemies?.Invoke(enemies);
    }

    public async void SetEnemyPath(List<TDEnemyView> enemies, List<IGridCellModel> paths)
    {
        await Task.Delay(TDConstant.CONFIG_ENEMY_SPAWN_INTERVAL * 1000);
        foreach (TDEnemyView enemy in enemies)
        {
            if (paths is { Count: > 0 })
            {
                if (enemy != null)
                {
                    enemy.SetPath(paths);
                }
                else
                {
                    if(Application.isPlaying)
                        Debug.Log("<color=red>EnemyController component not found on enemy prefab!</color>");
                }
            }

            await Task.Delay(TDConstant.CONFIG_ENEMY_SPAWN_INTERVAL * 1000);
        }
    }
}
