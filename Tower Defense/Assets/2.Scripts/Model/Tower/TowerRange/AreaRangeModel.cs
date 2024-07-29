using UnityEngine;

namespace Weapon.Range
{
    public class AreaRangeModel : IWeaponRangeModel
    {
        private readonly int m_Range;

        public AreaRangeModel(int getRange)
        {
            m_Range = getRange;
        }
        
        public bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation)
        {
            float distance = Vector3.Distance(towerPosition, enemyPosition);
            return distance <= m_Range;
        }
    }
}