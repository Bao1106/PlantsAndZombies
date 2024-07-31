using System;
using UnityEngine;

public class TDGameplayMainView : MonoBehaviour
{
    private TDEnemyPathMainView m_TDEnemyPathMainView;
    private GameObject m_MapVisualize;

    private void Awake() => InitPath();

    private void InitPath()
    {
        IGridModel gridModel = new TDGridModel(25, 25);
        IPathFinderModel pathfinder = new TDaStarPathControl();
        IEnemyFactoryModel enemyFactoryModel = new TDEnemyFactoryModel(pathfinder);

        //Init grid for enemy path
        m_MapVisualize = GameObject.Find(TDConstant.GAMEPLAY_MAP_VISUALIZE);
        Vector3 mapSize = m_MapVisualize.GetComponent<Renderer>().bounds.size;
        Vector3 planePosition = m_MapVisualize.transform.position;
        TDGridMainModel.Initialize(mapSize, planePosition);
        TDGridMainModel.api.CreateGrid();
        
        //Init enemy path
        m_TDEnemyPathMainView = transform.Find(TDConstant.GAMEPLAY_ENEMY_PATH_MAIN_VIEW).GetComponent<TDEnemyPathMainView>();
        TDGameplayMainControl.api.InitEnemyPath(m_TDEnemyPathMainView, gridModel, enemyFactoryModel);
    }
}
