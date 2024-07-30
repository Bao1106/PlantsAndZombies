using System;
using System.Collections.Generic;
using Grid_Manager;
using UnityEngine;
using Object = UnityEngine.Object;
public class TDEnemyPathControl
{
    public static TDEnemyPathControl api;

    public Action<List<GameObject>> onGetPaths;
    
    public void CreatePath(List<IGridCellModel> paths, IGridManager gridManager, GameObject pathPrefab)
    {
        List<GameObject> tiles = new List<GameObject>();
        
        foreach (IGridCellModel cell in paths)
        {
            Vector3 worldPosition = gridManager.GetGrid()[cell.Position.x, cell.Position.y];
            GameObject tile = Object.Instantiate(pathPrefab, worldPosition, Quaternion.identity);
            tile.transform.position =
                new Vector3(tile.transform.position.x, TDConstant.CONFIG_PATH_OFFSET_Y, tile.transform.position.z);
            tiles.Add(tile);
                
            gridManager.SetOccupiedCell(worldPosition);

            /*// Điều chỉnh rotation nếu cần
            if (i < path.Count - 1)
            {
                Vector3 nextPosition = new Vector3(path[i + 1].Position.x, tileOffsetY, path[i + 1].Position.y);
                Vector3 direction = nextPosition - tilePosition;
                tile.transform.forward = direction.normalized;
            }*/
        }
        
        onGetPaths?.Invoke(tiles);
    }
}
