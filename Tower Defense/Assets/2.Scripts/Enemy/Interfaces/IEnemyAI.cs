using System.Collections.Generic;
using Interfaces.Grid;
using UnityEngine;

namespace Enemy.Interfaces
{
    public interface IEnemyAI
    {
        List<IGridCell> CalculatePath(IGrid grid, IGridCell start, IGridCell end, List<Vector2Int> waypoints);
    }
}