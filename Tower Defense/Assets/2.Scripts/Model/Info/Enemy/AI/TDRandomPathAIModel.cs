using System.Collections.Generic;
using UnityEngine;

public class TDRandomPathAIModel : IEnemyAIModel
{
    private readonly IPathFinderModel m_PathFinderModel;

    public TDRandomPathAIModel(IPathFinderModel aiPathFinderModel)
    {
        m_PathFinderModel = aiPathFinderModel;
    }
        
    public void CalculatePath(IGridDTO gridDTO, IGridCellDTO start, IGridCellDTO end, Vector2Int[] waypoints)
    {
        Debug.LogError("Random");
        // Implement random path calculation using _pathFinder
        // You can use Random.Range to add some randomness to the path
        //return m_PathFinderModel.FindPath(gridDTO, start, end);
    }
    
    public void CalculateFinalPath(IGridDTO gridDTO, IGridCellDTO current, IGridCellDTO end)
    {
        
    }
}