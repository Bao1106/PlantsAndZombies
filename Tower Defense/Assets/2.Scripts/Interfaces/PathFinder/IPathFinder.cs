using System.Collections.Generic;
using Interfaces.Grid;

namespace Interfaces.PathFinder
{
    public interface IPathFinder
    {
        List<IGridCell> FindPath(IGrid grid, IGridCell start, IGridCell end);
        List<List<IGridCell>> FindMultiplePaths(IGrid grid, IGridCell start, IGridCell end, int numberOfPaths);
    }
}