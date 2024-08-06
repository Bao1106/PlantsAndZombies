using System.Collections.Generic;
using UnityEngine;

public class TDWaypointAIModel : IEnemyAIModel
{
    private readonly IPathFinderModel m_PathFinderModel;

    public TDWaypointAIModel(IPathFinderModel aiPathFinderModel)
    {
        m_PathFinderModel = aiPathFinderModel;
    }
        
    public void CalculatePath(IGridDTO gridDTO, IGridCellDTO start, IGridCellDTO end, Vector2Int[] waypoints)
    {
        //List<IGridCellDTO> path = new List<IGridCellDTO>();
        IGridCellDTO currentStart = start;

        foreach (Vector2Int waypoint in waypoints)
        {
            IGridCellDTO waypointCellDto = gridDTO.GetCell(waypoint.x, waypoint.y);
            //List<IGridCellDTO> segment = m_PathFinderModel.FindPath(gridDTO, currentStart, waypointCellDto);
            TDaStarPathControl.api.FindPath(gridDTO, currentStart, waypointCellDto, false);
        }
    }

    public void CalculateFinalPath(IGridDTO gridDTO, IGridCellDTO current, IGridCellDTO end)
    {
        TDaStarPathControl.api.FindPath(gridDTO, current, end, true);
    }
}