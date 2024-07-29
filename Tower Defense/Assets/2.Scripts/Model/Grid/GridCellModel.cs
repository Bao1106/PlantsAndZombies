using Enums;
using UnityEngine;

public class GridCellModel : IGridCellModel
{
    public Vector2Int Position { get; }
    public bool IsWalkable { get; set; }
    public CellType Type { get; set; }
        
    public GridCellModel(int x, int y, CellType type = CellType.Empty)
    {
        Position = new Vector2Int(x, y);
        IsWalkable = type != CellType.Obstacle;
        Type = type;
    }
}