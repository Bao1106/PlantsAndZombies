using System.Collections.Generic;
using Enemy.Interfaces;
using Interfaces.Grid;
using Interfaces.PathFinder;
using UnityEngine;

namespace Enemy
{
    public class RandomPathAI : IEnemyAI
    {
        private readonly IPathFinder pathFinder;

        public RandomPathAI(IPathFinder aiPathFinder)
        {
            pathFinder = aiPathFinder;
        }
        
        public List<IGridCell> CalculatePath(IGrid grid, IGridCell start, IGridCell end, List<Vector2Int> waypoints)
        {
            Debug.LogError("Random");
            // Implement random path calculation using _pathFinder
            // You can use Random.Range to add some randomness to the path
            return pathFinder.FindPath(grid, start, end);
        }
    }
}