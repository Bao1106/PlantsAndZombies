using TowerPlacer;
using UnityEngine;

namespace PlaceTowerCommand
{
    public class PlaceTowerCommand : IPlaceTowerCommand
    {
        private readonly ITowerPlacer towerPlacer;
        private readonly Vector3 position;
        
        public PlaceTowerCommand(ITowerPlacer placer, Vector3 placePosition)
        {
            towerPlacer = placer;
            position = placePosition;
        }
        
        public void Execute()
        {
            towerPlacer.PlaceTower(position);
        }
    }
}