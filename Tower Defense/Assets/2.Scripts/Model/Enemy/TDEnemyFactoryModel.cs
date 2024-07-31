using System;
using TDEnums;

public class TDEnemyFactoryModel : IEnemyFactoryModel
{
    private readonly IPathFinderModel m_PathFinderModel;

    public TDEnemyFactoryModel(IPathFinderModel aiPathFinderModel)
    {
        m_PathFinderModel = aiPathFinderModel;
    }
        
    public IEnemyAIModel CreateAI(EnemyAiType type)
    {
        switch (type)
        {
            case EnemyAiType.Waypoint:
                return new TDWaypointAIModel(m_PathFinderModel);
            case EnemyAiType.Random:
                return new TDRandomPathAIModel(m_PathFinderModel);
            default:
                throw new ArgumentException("Unknown AI type");
        }
    }
}