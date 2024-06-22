using UnityEngine;

namespace Grid_Manager
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;

        void OnDrawGizmos()
        {
            Vector3[,] grid = gridManager.GetGrid();
            if (grid == null) return;

            for (int x = 0; x < gridManager.Width; x++)
            {
                for (int z = 0; z < gridManager.Height; z++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(grid[x, z], new Vector3(gridManager.CellSize, 0.1f, gridManager.CellSize));
                }
            }
        }
    }
}
