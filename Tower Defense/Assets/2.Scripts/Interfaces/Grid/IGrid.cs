namespace Interfaces.Grid
{
    public interface IGrid
    {
        int width { get; }
        int height { get; }
        IGridCell GetCell(int x, int y);
        void SetCell(int x, int y, IGridCell cell);
    }
}