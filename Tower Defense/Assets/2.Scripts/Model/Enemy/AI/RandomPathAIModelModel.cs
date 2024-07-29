using System.Collections.Generic;
using UnityEngine;

public class RandomPathAIModelModel : IEnemyAIModel
{
    private readonly IPathFinderModel m_PathFinderModel;

    public RandomPathAIModelModel(IPathFinderModel aiPathFinderModel)
    {
        m_PathFinderModel = aiPathFinderModel;
    }
        
    public List<IGridCellModel> CalculatePath(IGridModel gridModel, IGridCellModel start, IGridCellModel end, List<Vector2Int> waypoints)
    {
        Debug.LogError("Random");
        // Implement random path calculation using _pathFinder
        // You can use Random.Range to add some randomness to the path
        return m_PathFinderModel.FindPath(gridModel, start, end);
    }
}