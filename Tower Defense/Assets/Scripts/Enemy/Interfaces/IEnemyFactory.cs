using Enums;

namespace Enemy.Interfaces
{
    public interface IEnemyFactory
    {
        IEnemyAI CreateAI(EnemyAiType type);
    }
}