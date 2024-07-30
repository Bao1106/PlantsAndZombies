using System;
using Grid_Manager;
using TowerFactory;
using UnityEngine;

public class TDPlaceTowerControl
{
    public static TDPlaceTowerControl api;

    public Action<bool> onPlaceTowerSuccess;
    
    public void CheckPlaceTower(Vector3 position, GameObject currentTower, IGridMainModel gridMainModel)
    {
        // Kiểm tra xem vị trí có hợp lệ không
        if (gridMainModel.IsValidPlacement(position))
        {
            Vector3 nearestPosition = gridMainModel.GetNearestGridPosition(position);
            Quaternion rotation = currentTower.transform.rotation;
            TDTowerFactoryControl.api.CreateTower(currentTower, nearestPosition, rotation);
            onPlaceTowerSuccess?.Invoke(true);
                
            gridMainModel.SetOccupiedCell(nearestPosition);
        }
    }
}
