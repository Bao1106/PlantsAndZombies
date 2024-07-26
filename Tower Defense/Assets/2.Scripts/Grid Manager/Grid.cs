using Interfaces.Grid;
using UnityEngine;

namespace Grid_Manager
{
    public class Grid : IGrid
    {
        private readonly IGridCell[,] cells;
        
        public int Width { get; }
        public int Height { get; }
        
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new IGridCell[width, height];
            InitializeGrid();
        }
        
        private void InitializeGrid()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    cells[x, y] = new GridCell(x, y);
                }
            }
        }
        
        public IGridCell GetCell(int x, int y)
        {
            return cells[x, y];
        }

        public void SetCell(int x, int y, IGridCell cell)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                Debug.LogError($"Attempted to set cell outside grid bounds: ({x}, {y})");
                return;
            }
            cells[x, y] = cell;
        }
    }
}