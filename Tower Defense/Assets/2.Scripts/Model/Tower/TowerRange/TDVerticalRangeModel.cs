using UnityEngine;

public class TDVerticalRangeModel : IWeaponRangeModel
{
    private readonly int m_Range;

    public TDVerticalRangeModel(int getRange)
    {
        m_Range = getRange;
    }
        
    public bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation)
    {
        Vector3 forward = towerRotation * Vector3.forward;
        Vector3 toEnemy = enemyPosition - towerPosition;
        float distance = Vector3.Distance(towerPosition, enemyPosition);
        return Vector3.Dot(forward, toEnemy.normalized) > 0.9f && distance <= m_Range;
    }
}