using System.Collections.Generic;
using Grid_Manager;
using Interfaces.Grid;
using Services.DependencyInjection;
using UnityEngine;

public class EnemyPathView : MonoBehaviour, IDependencyProvider
{
    [SerializeField] private float tileOffsetY = 0.1f;
    [SerializeField] private GameObject pathPrefab;

    [Inject] private IGridManager m_GridManager;
        
    private readonly List<GameObject> m_InstantiatedTiles = new List<GameObject>();

    [Provide]
    public EnemyPathView ProviderEnemyPathView() => this;
        
    public void VisualizePath(List<IGridCell> path)
    {
        ClearPreviousPath();

        foreach (var cell in path)
        {
            Vector3 worldPosition = m_GridManager.GetGrid()[cell.Position.x, cell.Position.y];
            var tile = Instantiate(pathPrefab, worldPosition, Quaternion.identity, transform.parent);
            tile.transform.position =
                new Vector3(tile.transform.position.x, tileOffsetY, tile.transform.position.z);
            m_InstantiatedTiles.Add(tile);
                
            m_GridManager.SetOccupiedCell(worldPosition);

            /*// Điều chỉnh rotation nếu cần
            if (i < path.Count - 1)
            {
                Vector3 nextPosition = new Vector3(path[i + 1].Position.x, tileOffsetY, path[i + 1].Position.y);
                Vector3 direction = nextPosition - tilePosition;
                tile.transform.forward = direction.normalized;
            }*/
        }
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