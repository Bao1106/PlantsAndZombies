using Enemy;
using UnityEngine;

namespace Weapon
{
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] private TowerWeapon towerWeapon;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                towerWeapon.SetTarget(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                towerWeapon.SetTarget(null);
            }
        }
    }
}