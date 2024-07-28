using Interfaces.Grid;
using UnityEngine;

namespace Grid_Manager
{
    public class Grid : IGrid
    {
        private readonly IGridCell[,] m_Cells;
        
        public int width { get; }
        public int height { get; }
        
        public Grid(int w, int h)
        {
            width = w;
            height = h;
            m_Cells = new IGridCell[w, h];
            InitializeGrid();
        }
        
        private void InitializeGrid()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    m_Cells[x, y] = new GridCell(x, y);
                }
            }
        }
        
        public IGridCell GetCell(int x, int y)
        {
            return m_Cells[x, y];
        }

        public void SetCell(int x, int y, IGridCell cell)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                Debug.LogError($"Attempted to set cell outside grid bounds: ({x}, {y})");
                return;
            }
            m_Cells[x, y] = cell;
        }
    }
}