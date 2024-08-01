using System.Collections.Generic;
using Services.DependencyInjection;
using UnityEngine;

public class TDEnemyPathMainView : MonoBehaviour
{
    private Vector2Int m_StartPoint, m_EndPoint;
    private Transform m_SpawnPos;
    
    private IGridModel m_GridModel;
    private IEnemyFactoryModel m_AIFactoryModel;
    private IEnemyAIModel m_EnemyAIModel;
    private TDEnemyPathView m_EnemyPathView;
    private TDEnemyView m_Slime; //prefab slime
    private List<IGridCellModel> m_CurrentPaths = new List<IGridCellModel>();
    private List<TDEnemyView> m_EnemiesView = new List<TDEnemyView>();
    
    public void Initialize(IGridModel initGridModel, IEnemyFactoryModel initFactoryModel)
    {
        m_StartPoint = TDConstant.CONFIG_ENEMY_START_POINT;
        m_EndPoint = TDConstant.CONFIG_ENEMY_END_POINT;
        m_SpawnPos = GameObject.Find(TDConstant.GAMEPLAY_ENEMY_PATH_CREATE_POINT).transform;
        m_EnemyPathView = GameObject.Find(TDConstant.GAMEPLAY_ENEMY_PATH_VIEW).GetComponent<TDEnemyPathView>();
        m_Slime = RepResourceObject.GetResource<GameObject>(TDConstant.PREFAB_SLIME).GetComponent<TDEnemyView>();
        
        m_GridModel = initGridModel;
        m_AIFactoryModel = initFactoryModel;
        m_EnemyAIModel = m_AIFactoryModel.CreateAI(m_Slime.AiType);

        ImplementPath();
    }

    private async void ImplementPath()
    {
        await TDInitializeModel.api.createGridCompletion.Task;
        m_EnemyPathView.RegistryValues();
        RegistryEvents();
        
        TDEnemyPathMainControl.api.InitEnemyPath(m_GridModel, m_StartPoint, m_EndPoint);
        TDEnemyPathMainControl.api.GenerateEnemyPath(m_EnemyAIModel, m_GridModel, m_StartPoint, m_EndPoint, m_EnemyPathView);
        TDEnemyPathMainControl.api.SpawnEnemies(m_Slime, m_SpawnPos);
        TDEnemyPathMainControl.api.SetEnemyPath(m_EnemiesView, m_CurrentPaths);
    }

    private void RegistryEvents()
    {
        TDEnemyPathMainControl.api.onGetEnemyPos += OnGetEnemyPos;
        TDEnemyPathMainControl.api.onGetEnemyPath += OnGetEnemyPath;
        TDEnemyPathMainControl.api.onGetEnemies += OnGetEnemies;
    }

    private void OnDestroy()
    {
        TDEnemyPathMainControl.api.onGetEnemyPos -= OnGetEnemyPos;
        TDEnemyPathMainControl.api.onGetEnemyPath -= OnGetEnemyPath;
        TDEnemyPathMainControl.api.onGetEnemies -= OnGetEnemies;
    }

    private void OnGetEnemyPos(Vector2Int startPoint, Vector2Int endPoint)
    {
        m_StartPoint = startPoint;
        m_EndPoint = endPoint;
    }

    private void OnGetEnemyPath(List<IGridCellModel> lstCell)
    {
        m_CurrentPaths = lstCell;
    }
    
    private void OnGetEnemies(List<TDEnemyView> enemies)
    {
        m_EnemiesView = enemies;
    }
}
