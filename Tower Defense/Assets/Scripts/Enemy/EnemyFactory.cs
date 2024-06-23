using System;
using Enemy.Interfaces;
using Enums;
using Interfaces.PathFinder;
using Services.DependencyInjection;

namespace Enemy
{
    public class EnemyFactory : IEnemyFactory, IDependencyProvider
    {
        private readonly IPathFinder pathFinder;

        public EnemyFactory(IPathFinder aiPathFinder)
        {
            pathFinder = aiPathFinder;
        }
        
        public IEnemyAI CreateAI(EnemyAiType type)
        {
            switch (type)
            {
                case EnemyAiType.Waypoint:
                    return new WaypointAI(pathFinder);
                case EnemyAiType.Random:
                    return new RandomPathAI(pathFinder);
                default:
                    throw new ArgumentException("Unknown AI type");
            }
        }
    }
}