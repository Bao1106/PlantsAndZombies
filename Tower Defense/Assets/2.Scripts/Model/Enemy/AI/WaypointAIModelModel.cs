using System.Collections.Generic;
using Interfaces.Grid;
using Interfaces.PathFinder;
using UnityEngine;

public class WaypointAIModelModel : IEnemyAIModel
{
    private readonly IPathFinder m_PathFinder;

    public WaypointAIModelModel(IPathFinder aiPathFinder)
    {
        m_PathFinder = aiPathFinder;
    }
        
    public List<IGridCell> CalculatePath(IGrid grid, IGridCell start, IGridCell end, List<Vector2Int> waypoints)
    {
        List<IGridCell> path = new List<IGridCell>();
        IGridCell currentStart = start;

        foreach (var waypoint in waypoints)
        {
            IGridCell waypointCell = grid.GetCell(waypoint.x, waypoint.y);
            List<IGridCell> segment = m_PathFinder.FindPath(grid, currentStart, waypointCell);
            if (segment != null)
            {
                path.AddRange(segment);
                currentStart = waypointCell;
            }
        }

        List<IGridCell> finalSegment = m_PathFinder.FindPath(grid, currentStart, end);
        if (finalSegment != null)
        {
            path.AddRange(finalSegment);
        }

        return path;
    }
}