using UnityEngine;

public class TDGameplayMainView : MonoBehaviour
{
    private TDEnemyPathMainView m_TDEnemyPathMainView;

    private void Awake() => InitPath();

    private void InitPath()
    {
        IGridModel gridModel = new TDGridModel(25, 25);
        IPathFinderModel pathfinder = new TDaStarPathControl();
        IEnemyFactoryModel enemyFactoryModel = new TDEnemyFactoryModel(pathfinder);

        m_TDEnemyPathMainView = transform
            .Find(TDConstant.GAMEPLAY_ENEMY_PATH_MAIN_VIEW)
            .GetComponent<TDEnemyPathMainView>();
        TDGameplayMainControl.api.InitEnemyPath(m_TDEnemyPathMainView, gridModel, enemyFactoryModel);
    }
}
