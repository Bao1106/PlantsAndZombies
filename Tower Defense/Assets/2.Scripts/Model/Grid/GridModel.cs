using UnityEngine;

public class GridModel : IGridModel
{
    private readonly IGridCellModel[,] m_Cells;
        
    public int width { get; }
    public int height { get; }
        
    public GridModel(int w, int h)
    {
        width = w;
        height = h;
        m_Cells = new IGridCellModel[w, h];
        InitializeGrid();
    }
        
    private void InitializeGrid()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                m_Cells[x, y] = new GridCellModel(x, y);
            }
        }
    }
        
    public IGridCellModel GetCell(int x, int y)
    {
        return m_Cells[x, y];
    }

    public void SetCell(int x, int y, IGridCellModel cellModel)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError($"Attempted to set cell outside grid bounds: ({x}, {y})");
            return;
        }
        m_Cells[x, y] = cellModel;
    }
}