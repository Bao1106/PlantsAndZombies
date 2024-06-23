namespace Interfaces.Grid
{
    public interface IGrid
    {
        int Width { get; }
        int Height { get; }
        IGridCell GetCell(int x, int y);
        void SetCell(int x, int y, IGridCell cell);
    }
}