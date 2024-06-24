using Enums;
using Services.DependencyInjection;
using UnityEngine;
using Weapon.Interfaces;

namespace Weapon
{
    public class TowerWeapon : MonoBehaviour
    {
        [SerializeField] private TowerType type;
        
        private IWeaponRange weaponRange;
        private Quaternion oriQuaternion;
        private Transform target;

        public TowerType GetTowerType => type;
        
        public void Initialize(IWeaponRange initWeapon)
        {
            weaponRange = initWeapon;
            oriQuaternion = transform.rotation;
        }

        private void Update()
        {
            if (target != null && weaponRange.IsInRange(transform.position, target.position, oriQuaternion))
            {
                RotateTowardsTarget();
            }
            else
            {
                ResetRotation();
            }
        }
        
        private void RotateTowardsTarget()
        {
            Vector3 targetDirection = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void ResetRotation()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, oriQuaternion, Time.deltaTime * 5f);
        }

        public void SetTarget(Transform setTarget)
        {
            target = setTarget;
        }
    }
}