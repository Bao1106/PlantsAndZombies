using UnityEngine;
using UnityEngine.Serialization;

namespace Grid_Manager
{
    public class TDGridVisualizer : MonoBehaviour
    {
        [FormerlySerializedAs("tdGridManager")]
        [FormerlySerializedAs("gridManager")]
        [SerializeField] private TDGridMainModel tdGridMainModel;

        void OnDrawGizmos()
        {
            Vector3[,] grid = tdGridMainModel.GetGrid();
            if (grid == null) return;

            for (int x = 0; x < tdGridMainModel.width; x++)
            {
                for (int z = 0; z < tdGridMainModel.height; z++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(grid[x, z], new Vector3(tdGridMainModel.cellSize, 0.1f, tdGridMainModel.cellSize));
                }
            }
        }
    }
}
