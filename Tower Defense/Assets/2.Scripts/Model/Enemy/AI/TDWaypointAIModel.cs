using System.Collections.Generic;
using UnityEngine;

public class TDWaypointAIModel : IEnemyAIModel
{
    private readonly IPathFinderModel m_PathFinderModel;

    public TDWaypointAIModel(IPathFinderModel aiPathFinderModel)
    {
        m_PathFinderModel = aiPathFinderModel;
    }
        
    public List<IGridCellModel> CalculatePath(IGridModel gridModel, IGridCellModel start, IGridCellModel end, List<Vector2Int> waypoints)
    {
        List<IGridCellModel> path = new List<IGridCellModel>();
        IGridCellModel currentStart = start;

        foreach (var waypoint in waypoints)
        {
            IGridCellModel waypointCellModel = gridModel.GetCell(waypoint.x, waypoint.y);
            List<IGridCellModel> segment = m_PathFinderModel.FindPath(gridModel, currentStart, waypointCellModel);
            if (segment != null)
            {
                path.AddRange(segment);
                currentStart = waypointCellModel;
            }
        }

        List<IGridCellModel> finalSegment = m_PathFinderModel.FindPath(gridModel, currentStart, end);
        if (finalSegment != null)
        {
            path.AddRange(finalSegment);
        }

        return path;
    }
}