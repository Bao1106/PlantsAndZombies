using System.Collections.Generic;
using Interfaces.Grid;
using UnityEngine;

public interface IEnemyAIModel
{
    List<IGridCell> CalculatePath(IGrid grid, IGridCell start, IGridCell end, List<Vector2Int> waypoints);
}