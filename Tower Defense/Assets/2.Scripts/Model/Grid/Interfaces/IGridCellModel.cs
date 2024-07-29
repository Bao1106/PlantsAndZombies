using Enums;
using UnityEngine;

public interface IGridCellModel
{
    Vector2Int Position { get; }
    bool IsWalkable { get; set; }
    CellType Type { get; set; }
}