using System.Collections.Generic;
using System.Linq;
using TDEnums;
using UnityEngine;

// ReSharper disable once InconsistentNaming
public class TDaStarPathControl : IPathFinderModel
{
    public List<IGridCellModel> FindPath(IGridModel gridModel, IGridCellModel start, IGridCellModel end)
    {
        List<IGridCellModel> openSet = new List<IGridCellModel> { start };
        List<IGridCellModel> closedSet = new List<IGridCellModel>();
        Dictionary<IGridCellModel, IGridCellModel> cameFrom = new Dictionary<IGridCellModel, IGridCellModel>();
        Dictionary<IGridCellModel, float> gScore = new Dictionary<IGridCellModel, float>();
        Dictionary<IGridCellModel, float> fScore = new Dictionary<IGridCellModel, float>();

        gScore[start] = 0;
        fScore[start] = HeuristicCostEstimate(start, end);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(node => fScore[node]).First();

            if (current.Position == end.Position)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(gridModel, current))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                var tentativeGScore = gScore[current] + GetMovementCost(current, neighbor);

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, end);
            }
        }

        return null;
    }

    private float HeuristicCostEstimate(IGridCellModel start, IGridCellModel goal)
    {
        Vector2 startPos = start.Position;
        Vector2 goalPos = goal.Position;
        float dx = Mathf.Abs(startPos.x - goalPos.x);
        float dy = Mathf.Abs(startPos.y - goalPos.y);
        return (Mathf.Max(dx, dy) + Random.Range(0f, 0.1f)) * (1 + Random.Range(0f, 0.1f));
    }

    private float GetMovementCost(IGridCellModel from, IGridCellModel to)
    {
        // Chỉ cho phép di chuyển ngang hoặc dọc
        return 1.0f;
    }
    
    private List<IGridCellModel> GetNeighbors(IGridModel gridModel, IGridCellModel cellModel)
    {
        List<IGridCellModel> neighbors = new List<IGridCellModel>();
        int[] dx = { 0, 1, 0, -1 }; // Chỉ cho phép di chuyển lên, phải, xuống, trái
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            var checkX = cellModel.Position.x + dx[i];
            var checkY = cellModel.Position.y + dy[i];

            if (checkX >= 0 && checkX < gridModel.width && checkY >= 0 && checkY < gridModel.height)
            {
                var neighbor = gridModel.GetCell(checkX, checkY);
                if (neighbor.IsWalkable && neighbor.Type != CellType.Obstacle)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }

    private List<IGridCellModel> ReconstructPath(Dictionary<IGridCellModel, IGridCellModel> cameFrom, IGridCellModel current)
    {
        List<IGridCellModel> path = new List<IGridCellModel> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private void MarkPathAsNonWalkable(IGridModel gridModel, List<IGridCellModel> path)
    {
        foreach (IGridCellModel cell in path)
        {
            cell.IsWalkable = false; // Mark the path cells as non-walkable
        }
    }

    private void ResetPathAsWalkable(IGridModel gridModel, List<IGridCellModel> path)
    {
        foreach (IGridCellModel cell in path)
        {
            cell.IsWalkable = true; // Reset the path cells to walkable
        }
    }

    public List<List<IGridCellModel>> FindMultiplePaths(IGridModel gridModel, IGridCellModel start, IGridCellModel end, int numberOfPaths)
    {
        List<List<IGridCellModel>> paths = new List<List<IGridCellModel>>();
        for (int i = 0; i < numberOfPaths; i++)
        {
            var path = FindPath(gridModel, start, end);
            if (path != null)
            {
                paths.Add(path);
                MarkPathAsNonWalkable(gridModel, path);
            }
            else
            {
                break;
            }
        }

        // Reset all paths to walkable after finding all paths
        foreach (List<IGridCellModel> path in paths)
        {
            ResetPathAsWalkable(gridModel, path);
        }

        return paths;
    }
}
