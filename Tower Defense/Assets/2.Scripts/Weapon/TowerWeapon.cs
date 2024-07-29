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
        private Quaternion m_OriQuaternion;
        private Transform m_Target;
        private float m_LastAttackTime;

        public TowerType GetTowerType
        {
            get
            {
                return type;
            }
        }

        public void Initialize(IWeaponRangeModel initWeaponRangeModel, IWeaponModel initWeaponModel)
        {
            m_WeaponRangeModel = initWeaponRangeModel;
            m_WeaponModel = initWeaponModel;
            m_OriQuaternion = transform.rotation;

            m_WeaponModel.GetType(type);
        }

        private void Update()
        {
            if(m_WeaponRangeModel == null) return;
            
            if (m_Target != null && m_WeaponRangeModel.IsInRange(transform.position, m_Target.position, m_OriQuaternion))
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
            Vector3 targetDirection = m_Target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void ResetRotation()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, m_OriQuaternion, Time.deltaTime * 5f);
        }

        private void AttackTarget()
        {
            if (Time.time - m_LastAttackTime >= 1f / m_WeaponModel.GetAttackSpeed())
            {
                m_WeaponModel.Attack(m_Target, spawnBullet);
                m_LastAttackTime = Time.time;
            }
        }
        
        public void SetTarget(Transform setTarget)
        {
            m_Target = setTarget;
        }
    }
}