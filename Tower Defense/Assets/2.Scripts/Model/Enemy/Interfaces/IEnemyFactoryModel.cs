using Enums;

public interface IEnemyFactoryModel
{
    IEnemyAIModel CreateAI(EnemyAiType type);
}