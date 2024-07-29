using System;
using PlaceTowerCommand;
using Services.DependencyInjection;
using TowerFactory;
using TowerPlacer;
using UnityEngine;

namespace InputHandler
{
    public class UserInputHandler : MonoBehaviour
    {
        [Inject] private ITowerPlacer towerPlacer;
        [Inject] private TowerMainView m_TowerMainView;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Camera.main == null) return;
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    IPlaceTowerCommand command = new PlaceTowerCommand.PlaceTowerCommand(towerPlacer, hit.point);
                    command.Execute();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E)) // Press E to rotate clockwise
            {
                m_TowerMainView.RotateTowerClockwise();
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Press Q to rotate counterclockwise
            {
                m_TowerMainView.RotateTowerCounterclockwise();
            }
            else if (Input.GetMouseButtonDown(1)) // Right click to cancel placement
            {
                m_TowerMainView.CancelPlacement();
            }
        }
    }
}