using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAIModel
{
    List<IGridCellModel> CalculatePath(IGridModel gridModel, IGridCellModel start, IGridCellModel end, List<Vector2Int> waypoints);
}