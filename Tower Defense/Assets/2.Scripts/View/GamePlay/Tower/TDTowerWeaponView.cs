using TDEnums;
using UnityEngine;

public class TDTowerWeaponView : MonoBehaviour
{
    [SerializeField] private TowerType type;

    private TDTowerWeaponControl m_WeaponControl;
    private IWeaponRangeModel m_WeaponRangeModel;
    private IWeaponModel m_WeaponModel;
    private Quaternion m_OriQuaternion;
    private Transform m_Target, m_SpawnBullet;
    private float m_LastAttackTime;

    public TowerType GetTowerType
    {
        get
        {
            return type;
        }
    }
    
    public void Init(IWeaponRangeModel initWeaponRangeModel, IWeaponModel initWeaponModel, TDTowerWeaponControl weaponControl)
    {
        m_WeaponRangeModel = initWeaponRangeModel;
        m_WeaponModel = initWeaponModel;
        m_OriQuaternion = transform.rotation;

        m_SpawnBullet = transform.Find(TDConstant.GAMEPLAY_TOWER_BULLET_SPAWN);
        
        m_WeaponModel.GetType(type);

        m_WeaponControl = weaponControl;
        m_WeaponControl.onGetLastAttackTime += OnGetLastAttackTime;
    }

    private void OnDestroy()
    {
        if (m_WeaponControl != null)
        {
            m_WeaponControl.onGetLastAttackTime -= OnGetLastAttackTime;
            
        }
    }

    private void OnGetLastAttackTime(float time)
    {
        m_LastAttackTime = time;
    }

    private void Update()
    {
        if(m_WeaponControl == null) return;
        
        if (m_Target != null && m_WeaponRangeModel.IsInRange(transform.position, m_Target.position, m_OriQuaternion))
        {
            RotateTowardsTarget();
            m_WeaponControl.AttackTarget(m_LastAttackTime, m_WeaponModel, m_Target, m_SpawnBullet);
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
    
    public void SetTarget(Transform setTarget)
    {
        m_Target = setTarget;
    }
}