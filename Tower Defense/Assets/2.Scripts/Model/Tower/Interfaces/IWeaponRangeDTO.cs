using UnityEngine;

public interface IWeaponRangeDTO
{
    bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation);
}