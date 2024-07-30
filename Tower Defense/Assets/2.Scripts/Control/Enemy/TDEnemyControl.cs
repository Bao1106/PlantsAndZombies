using System;
using System.Collections.Generic;
using UnityEngine;

public class TDEnemyControl
{
    public Action<List<Vector3>, int> onGetEnemyPathPos;
    
    public void SetEnemyPath(List<IGridCellModel> path, IGridMainModel gridMainModel)
    {
        List<Vector3> pos = new List<Vector3>();
        foreach (var cell in path)
        {
            Vector3 worldPosition = gridMainModel.GetGrid()[cell.Position.x, cell.Position.y];
            //pathPositions.Add(new Vector3(cell.Position.x, transform.position.y, cell.Position.y));
            pos.Add(worldPosition);
        }

        int curPathIndex = 0;
        onGetEnemyPathPos?.Invoke(pos, curPathIndex);
    }
}
