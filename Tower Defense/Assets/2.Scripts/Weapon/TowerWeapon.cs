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
        
        private IWeaponRangeModel m_WeaponRangeModel;
        private IWeaponModel m_WeaponModel;
        private Quaternion oriQuaternion;
        private Transform target;
        private float lastAttackTime;

        public TowerType GetTowerType => type;
        
        public void Initialize(IWeaponRangeModel initWeaponRangeModel, IWeaponModel initWeaponModel)
        {
            m_WeaponRangeModel = initWeaponRangeModel;
            m_WeaponModel = initWeaponModel;
            oriQuaternion = transform.rotation;

            m_WeaponModel.GetType(type);
        }

        private void Update()
        {
            if(m_WeaponRangeModel == null) return;
            
            if (target != null && m_WeaponRangeModel.IsInRange(transform.position, target.position, oriQuaternion))
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
            if (Time.time - lastAttackTime >= 1f / m_WeaponModel.GetAttackSpeed())
            {
                m_WeaponModel.Attack(target, spawnBullet);
                lastAttackTime = Time.time;
            }
        }
        
        public void SetTarget(Transform setTarget)
        {
            target = setTarget;
        }
    }
}