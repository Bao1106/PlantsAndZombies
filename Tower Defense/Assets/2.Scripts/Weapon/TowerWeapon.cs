using Enemy;
using Enums;
using Services.DependencyInjection;
using UnityEngine;
using Weapon.Interfaces;

namespace Weapon
{
    public class TowerWeapon : MonoBehaviour
    {
        [SerializeField] private TowerType type;
        [SerializeField] private Transform spawnBullet;
        
        private IWeaponRange weaponRange;
        private IWeapon weapon;
        private Quaternion oriQuaternion;
        private Transform target;
        private float lastAttackTime;

        public TowerType GetTowerType => type;
        
        public void Initialize(IWeaponRange initWeaponRange, IWeapon initWeapon)
        {
            weaponRange = initWeaponRange;
            weapon = initWeapon;
            oriQuaternion = transform.rotation;

            weapon.GetType(type);
        }

        private void Update()
        {
            if(weaponRange == null) return;
            
            if (target != null && weaponRange.IsInRange(transform.position, target.position, oriQuaternion))
            {
                RotateTowardsTarget();
                AttackTarget();
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

        private void AttackTarget()
        {
            if (Time.time - lastAttackTime >= 1f / weapon.GetAttackSpeed())
            {
                weapon.Attack(target, spawnBullet);
                lastAttackTime = Time.time;
            }
        }
        
        public void SetTarget(Transform setTarget)
        {
            target = setTarget;
        }
    }
}