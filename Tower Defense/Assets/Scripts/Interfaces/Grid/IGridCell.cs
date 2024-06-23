using Enums;
using UnityEngine;

namespace Interfaces.Grid
{
    public interface IGridCell
    {
        Vector2Int Position { get; }
        bool IsWalkable { get; set; }
        CellType Type { get; set; }
    }
}