using TDEnums;
using UnityEngine;

public class TDGridCellDto : IGridCellDTO
{
    public Vector2Int position { get; }
    public bool isWalkable { get; set; }
    public CellType type { get; set; }
        
    public TDGridCellDto(int x, int y, CellType type = CellType.Empty)
    {
        position = new Vector2Int(x, y);
        isWalkable = type != CellType.Obstacle;
        this.type = type;
    }
}