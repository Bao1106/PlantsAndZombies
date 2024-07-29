using System;
using Enums;

public class EnemyFactoryModelModel : IEnemyFactoryModel
{
    private readonly IPathFinderModel m_PathFinderModel;

    public EnemyFactoryModelModel(IPathFinderModel aiPathFinderModel)
    {
        m_PathFinderModel = aiPathFinderModel;
    }
        
    public IEnemyAIModel CreateAI(EnemyAiType type)
    {
        switch (type)
        {
            case EnemyAiType.Waypoint:
                return new WaypointAIModelModel(m_PathFinderModel);
            case EnemyAiType.Random:
                return new RandomPathAIModelModel(m_PathFinderModel);
            default:
                throw new ArgumentException("Unknown AI type");
        }
    }
}