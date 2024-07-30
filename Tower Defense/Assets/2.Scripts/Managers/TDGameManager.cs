using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class TDGameManager : MonoBehaviour
{
    [FormerlySerializedAs("enemyPathManager")]
    [SerializeField] private TDEnemyPathManager tdEnemyPathManager;

    private void Awake()
    {
        IGridModel gridModel = new TDGridModel(25, 25);
        IPathFinderModel pathfinder = new TDaStarPathControl();
        IEnemyFactoryModel enemyFactoryModel = new TDEnemyFactoryModel(pathfinder);

        tdEnemyPathManager.Initialize(gridModel, enemyFactoryModel);
    }
}
