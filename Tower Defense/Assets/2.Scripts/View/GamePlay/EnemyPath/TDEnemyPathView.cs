using System;
using System.Collections.Generic;
using Grid_Manager;
using Services.DependencyInjection;
using UnityEngine;

public class TDEnemyPathView : MonoBehaviour
{
    [Inject] private IGridManager m_GridManager;
    private GameObject m_PathPrefab;
    private float m_PathOffsetY;
    
    private List<GameObject> m_InstantiatedTiles = new List<GameObject>();

    public void RegistryValues()
    {
        m_PathPrefab = ResourceObject.GetResource<GameObject>(TDConstant.PREFAB_PATH);
        TDEnemyPathControl.api.onGetPaths += OnGetPaths;
    }

    private void OnDestroy()
    {
        TDEnemyPathControl.api.onGetPaths -= OnGetPaths;
    }

    private void OnGetPaths(List<GameObject> paths)
    {
        m_InstantiatedTiles = paths;
    }

    public void VisualizePath(List<IGridCellModel> path)
    {
        ClearPreviousPath();
        TDEnemyPathControl.api.CreatePath(path, m_GridManager, m_PathPrefab);
    }

    private void ClearPreviousPath()
    {
        foreach (var tile in m_InstantiatedTiles)
        {
            Destroy(tile);
        }
        m_InstantiatedTiles.Clear();
    }
}