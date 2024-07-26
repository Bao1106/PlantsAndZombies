namespace TowerFactory
{
    public interface ITowerManipulator
    {
        void RotateTowerClockwise();
        void RotateTowerCounterclockwise();
        void CancelPlacement();
    }
}