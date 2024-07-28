using System;
using Enemy.Interfaces;
using Enums;
using Interfaces.PathFinder;
using Services.DependencyInjection;

namespace Enemy
{
    public class EnemyFactory : IEnemyFactory, IDependencyProvider
    {
        private readonly IPathFinder m_PathFinder;

        public EnemyFactory(IPathFinder aiPathFinder)
        {
            m_PathFinder = aiPathFinder;
        }
        
        public IEnemyAI CreateAI(EnemyAiType type)
        {
            switch (type)
            {
                case EnemyAiType.Waypoint:
                    return new WaypointAI(m_PathFinder);
                case EnemyAiType.Random:
                    return new RandomPathAI(m_PathFinder);
                default:
                    throw new ArgumentException("Unknown AI type");
            }
        }
    }
}