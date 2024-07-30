using UnityEngine;
using UnityEngine.Serialization;

namespace Grid_Manager
{
    public class TDGridVisualizer : MonoBehaviour
    {
        [FormerlySerializedAs("gridManager")]
        [SerializeField] private TDGridManager tdGridManager;

        void OnDrawGizmos()
        {
            Vector3[,] grid = tdGridManager.GetGrid();
            if (grid == null) return;

            for (int x = 0; x < tdGridManager.width; x++)
            {
                for (int z = 0; z < tdGridManager.height; z++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(grid[x, z], new Vector3(tdGridManager.cellSize, 0.1f, tdGridManager.cellSize));
                }
            }
        }
    }
}
