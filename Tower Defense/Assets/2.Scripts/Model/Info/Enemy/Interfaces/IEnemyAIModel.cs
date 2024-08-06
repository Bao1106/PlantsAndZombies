using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAIModel
{
    void CalculatePath(IGridDTO gridDTO, IGridCellDTO start, IGridCellDTO end, Vector2Int[] waypoints);
    void CalculateFinalPath(IGridDTO gridDTO, IGridCellDTO current, IGridCellDTO end);
}