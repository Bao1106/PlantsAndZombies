using TDEnums;

public interface IEnemyFactoryModel
{
    IEnemyAIModel CreateAI(EnemyAiType type);
}