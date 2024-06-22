using System;
using PlaceTowerCommand;
using Services.DependencyInjection;
using TowerPlacer;
using UnityEngine;

namespace InputHandler
{
    public class UserInputHandler : MonoBehaviour
    {
        [Inject] private ITowerPlacer towerPlacer;

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
        }
    }
}