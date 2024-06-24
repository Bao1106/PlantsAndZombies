using UnityEngine;
using Weapon.Interfaces;

namespace Weapon.Range
{
    public class VerticalRange : IWeaponRange
    {
        private readonly int range;

        public VerticalRange(int getRange)
        {
            range = getRange;
        }
        
        public bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation)
        {
            Vector3 forward = towerRotation * Vector3.forward;
            Vector3 toEnemy = enemyPosition - towerPosition;
            float distance = Vector3.Distance(towerPosition, enemyPosition);
            return Vector3.Dot(forward, toEnemy.normalized) > 0.9f && distance <= range;
        }
    }
}