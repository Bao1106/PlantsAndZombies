using Enums;
using Interfaces.Grid;
using UnityEngine;

namespace Grid_Manager
{
    public class GridCell : IGridCell
    {
        public Vector2Int Position { get; }
        public bool IsWalkable { get; set; }
        public CellType Type { get; set; }
        
        public GridCell(int x, int y, CellType type = CellType.Empty)
        {
            Position = new Vector2Int(x, y);
            IsWalkable = type != CellType.Obstacle;
            Type = type;
        }
    }
}