using UnityEngine;

public interface IWeaponRangeModel
{
    bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation);
}