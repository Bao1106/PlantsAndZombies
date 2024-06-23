using System.Collections.Generic;
using Enemy.Interfaces;
using Interfaces.Grid;
using Interfaces.PathFinder;
using UnityEngine;

namespace Enemy
{
    public class WaypointAI : IEnemyAI
    {
        private readonly IPathFinder pathFinder;

        public WaypointAI(IPathFinder aiPathFinder)
        {
            pathFinder = aiPathFinder;
        }
        
        public List<IGridCell> CalculatePath(IGrid grid, IGridCell start, IGridCell end, List<Vector2Int> waypoints)
        {
            Debug.LogError("Waypoint");
            var path = new List<IGridCell>();
            IGridCell currentStart = start;

            foreach (var waypoint in waypoints)
            {
                var waypointCell = grid.GetCell(waypoint.x, waypoint.y);
                var segment = pathFinder.FindPath(grid, currentStart, waypointCell);
                if (segment != null)
                {
                    path.AddRange(segment);
                    currentStart = waypointCell;
                }
            }

            var finalSegment = pathFinder.FindPath(grid, currentStart, end);
            if (finalSegment != null)
            {
                path.AddRange(finalSegment);
            }

            return path;
        }
    }
}