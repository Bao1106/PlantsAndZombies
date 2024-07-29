using Managers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyPathManager enemyPathManager;

    private void Awake()
    {
        IGridModel gridModel = new GridModel(25, 25);
        IPathFinderModel pathfinder = new AStarPathControl();
        IEnemyFactoryModel enemyFactoryModel = new EnemyFactoryModelModel(pathfinder);

        enemyPathManager.Initialize(gridModel, pathfinder, enemyFactoryModel);
    }
}
