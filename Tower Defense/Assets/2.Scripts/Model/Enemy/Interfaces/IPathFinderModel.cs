using System.Collections.Generic;

public interface IPathFinderModel
{
    List<IGridCellModel> FindPath(IGridModel gridModel, IGridCellModel start, IGridCellModel end);
    List<List<IGridCellModel>> FindMultiplePaths(IGridModel gridModel, IGridCellModel start, IGridCellModel end, int numberOfPaths);
}