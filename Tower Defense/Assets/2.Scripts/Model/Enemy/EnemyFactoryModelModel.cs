using System;
using Enemy;
using Enums;
using Interfaces.PathFinder;

public class EnemyFactoryModelModel : IEnemyFactoryModel
{
    private readonly IPathFinder m_PathFinder;

    public EnemyFactoryModelModel(IPathFinder aiPathFinder)
    {
        m_PathFinder = aiPathFinder;
    }
        
    public IEnemyAIModel CreateAI(EnemyAiType type)
    {
        switch (type)
        {
            case EnemyAiType.Waypoint:
                return new WaypointAIModelModel(m_PathFinder);
            case EnemyAiType.Random:
                return new RandomPathAIModelModel(m_PathFinder);
            default:
                throw new ArgumentException("Unknown AI type");
        }
    }
}