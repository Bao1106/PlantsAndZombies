using TDEnums;
using UnityEngine;

public class TDGridCellModel : IGridCellModel
{
    public Vector2Int Position { get; }
    public bool IsWalkable { get; set; }
    public CellType Type { get; set; }
        
    public TDGridCellModel(int x, int y, CellType type = CellType.Empty)
    {
        Position = new Vector2Int(x, y);
        IsWalkable = type != CellType.Obstacle;
        Type = type;
    }
}