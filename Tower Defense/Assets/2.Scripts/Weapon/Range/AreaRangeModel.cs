using UnityEngine;
using Weapon.Interfaces;

namespace Weapon.Range
{
    public class AreaRangeModel : IWeaponRangeModel
    {
        private readonly int range;

        public AreaRangeModel(int getRange)
        {
            range = getRange;
        }
        
        public bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation)
        {
            var distance = Vector3.Distance(towerPosition, enemyPosition);
            return distance <= range;
        }
    }
}