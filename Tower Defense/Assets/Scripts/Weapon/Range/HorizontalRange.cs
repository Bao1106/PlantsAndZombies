using UnityEngine;
using Weapon.Interfaces;

namespace Weapon.Range
{
    public class HorizontalRange : IWeaponRange
    {
        private readonly int range;

        public HorizontalRange(int getRange)
        {
            range = getRange;
        }
        
        public bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation)
        {
            var forward = towerRotation * Vector3.forward;
            var toEnemy = enemyPosition - towerPosition;
            var distance = Vector3.Distance(towerPosition, enemyPosition);
            return Vector3.Dot(forward, toEnemy.normalized) > 0.9f && distance <= range;
        }
    }
}