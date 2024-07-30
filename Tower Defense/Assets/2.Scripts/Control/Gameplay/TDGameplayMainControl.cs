public class TDGameplayMainControl
{
    public static TDGameplayMainControl api;

    public void InitEnemyPath(TDEnemyPathMainView enemyPath, IGridModel gridModel, IEnemyFactoryModel enemyFactoryModel)
    {
        enemyPath.Initialize(gridModel, enemyFactoryModel);
    }
}
