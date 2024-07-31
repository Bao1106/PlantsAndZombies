using TDEnums;
using UnityEngine;

public interface IGridCellModel
{
    Vector2Int position { get; }
    bool isWalkable { get; set; }
    CellType type { get; set; }
}