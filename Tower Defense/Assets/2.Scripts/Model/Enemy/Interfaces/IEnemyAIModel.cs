using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAIModel
{
    List<IGridCellModel> CalculatePath(IGridModel gridModel, IGridCellModel start, IGridCellModel end, Vector2Int[] waypoints);
}