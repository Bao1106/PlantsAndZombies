
using System.Threading.Tasks;
using UnityEngine;

namespace Grid_Manager
{
    public interface IGridManager
    {
        Vector3 GetNearestGridPosition(Vector3 worldPosition);
        Vector3[,] GetGrid();
        void SetOccupiedCell(Vector3 position);
        bool IsValidPlacement(Vector3 position);
        int Width { get; }
        int Height { get; }
        float CellSize { get; }
    }
}