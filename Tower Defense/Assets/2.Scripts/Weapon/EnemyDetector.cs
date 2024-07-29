using Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    public class EnemyDetector : MonoBehaviour
    {
        [FormerlySerializedAs("towerWeapon")]
        [SerializeField] private TowerWeaponView towerWeaponView;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                towerWeaponView.SetTarget(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                towerWeaponView.SetTarget(null);
            }
        }
    }
}