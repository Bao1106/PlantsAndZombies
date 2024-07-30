using System;
using Grid_Manager;
using TowerFactory;
using UnityEngine;

public class TDPlaceTowerControl
{
    public static TDPlaceTowerControl api;

    public Action<bool> onPlaceTowerSuccess;
    
    public void CheckPlaceTower(Vector3 position, GameObject currentTower, IGridManager gridManager)
    {
        // Kiểm tra xem vị trí có hợp lệ không
        if (gridManager.IsValidPlacement(position))
        {
            Vector3 nearestPosition = gridManager.GetNearestGridPosition(position);
            Quaternion rotation = currentTower.transform.rotation;
            TDTowerFactoryControl.api.CreateTower(currentTower, nearestPosition, rotation);
            onPlaceTowerSuccess?.Invoke(true);
                
            gridManager.SetOccupiedCell(nearestPosition);
        }
    }
}