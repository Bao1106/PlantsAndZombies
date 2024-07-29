using System.Collections.Generic;
using Interfaces.Grid;
using Interfaces.PathFinder;
using UnityEngine;

public class RandomPathAIModelModel : IEnemyAIModel
{
    private readonly IPathFinder m_PathFinder;

    public RandomPathAIModelModel(IPathFinder aiPathFinder)
    {
        m_PathFinder = aiPathFinder;
    }
        
    public List<IGridCell> CalculatePath(IGrid grid, IGridCell start, IGridCell end, List<Vector2Int> waypoints)
    {
        Debug.LogError("Random");
        // Implement random path calculation using _pathFinder
        // You can use Random.Range to add some randomness to the path
        return m_PathFinder.FindPath(grid, start, end);
    }
}