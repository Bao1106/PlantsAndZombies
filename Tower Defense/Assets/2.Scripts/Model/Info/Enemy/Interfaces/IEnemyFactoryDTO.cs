using TDEnums;

public interface IEnemyFactoryDTO
{
    IEnemyAIModel CreateAI(EnemyAiType type);
}