public interface IGridModel
{
    int width { get; }
    int height { get; }
    IGridCellModel GetCell(int x, int y);
    void SetCell(int x, int y, IGridCellModel cellModel);
}
