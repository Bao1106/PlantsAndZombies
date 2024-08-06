using System.Collections.Generic;

public interface IPathFinderModel
{
    void FindPath(IGridDTO gridDTO, IGridCellDTO start, IGridCellDTO end, bool isFinal);
    //List<IGridCellModel> FindPath(IGridModel gridModel, IGridCellModel start, IGridCellModel end);
    //List<List<IGridCellModel>> FindMultiplePaths(IGridModel gridModel, IGridCellModel start, IGridCellModel end, int numberOfPaths);
}