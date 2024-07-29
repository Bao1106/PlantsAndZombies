using System;
using Grid_Manager;
using UnityEngine;

public class TowerMainControl
{
    public static TowerMainControl api;

    public Action<string> onGetTowerName;
    public Action<int> onGetCurrentRotationIndex;
    public Action<int> onGetTowerCost;
    
    public void OnSelectTowerHolder(int index)
    {
        string towerName = string.Empty;
        
        switch (index)
        {
            case 0:
                towerName = DTConstant.PREFAB_FATTY_CANNON_G02;
                break;
            case 1:
                towerName = DTConstant.PREFAB_FATTY_CATAPULT_G02;
                break;
            case 2:
                towerName = DTConstant.PREFAB_FATTY_MISSILE_G02;
                break;
            case 3:
                towerName = DTConstant.PREFAB_FATTY_MISSILE_G03;
                break;
            case 4:
                towerName = DTConstant.PREFAB_FATTY_MORTAR_G02;
                break;
        }
        
        onGetTowerName?.Invoke(towerName);
    }

    public void OnSelectTower(GameObject currentTower, IGridManager gridManager)
    {
        if (currentTower != null)
        {
            if (Camera.main == null) return;
                
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Vector3 gridPosition = gridManager.GetNearestGridPosition(hit.point);
                currentTower.transform.position = gridPosition;
            }
        }
    }

    public void OnPlaceTower(GameObject currentTower, IGridManager gridManager)
    {
        if (currentTower != null)
        {
            if (Camera.main == null) return;
                
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                PlaceTowerControl.api.CheckPlaceTower(hit.point, currentTower, gridManager);
            }
        }
    }
    
    public void RotateTowerClockwise(GameObject currentTower, int currentRotationIndex)
    {
        if (currentTower != null)
        {
            int currentRotation = (currentRotationIndex + 1) % 4;
            UpdateTowerRotation(currentTower, currentRotation);
            onGetCurrentRotationIndex?.Invoke(currentRotation);
        }
    }

    public void RotateTowerCounterClockwise(GameObject currentTower, int currentRotationIndex)
    {
        if (currentTower != null)
        {
            int currentRotation = (currentRotationIndex - 1 + 4) % 4;
            UpdateTowerRotation(currentTower, currentRotation);
            onGetCurrentRotationIndex?.Invoke(currentRotation);
        }
    }
    
    private void UpdateTowerRotation(GameObject currentTower, int currentRotationIndex)
    {
        currentTower.transform.rotation = Quaternion.Euler(0f, DTConstant.GAMEPLAY_TOWER_ROTATIONS[currentRotationIndex], 0f);
    }
}
