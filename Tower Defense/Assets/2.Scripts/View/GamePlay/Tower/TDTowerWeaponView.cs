using TDEnums;
using UnityEngine;

public class TDTowerWeaponView : MonoBehaviour
{
    [SerializeField] private TowerType type;
    
    private IWeaponRangeModel m_WeaponRangeModel;
    private IWeaponModel m_WeaponModel;
    private Quaternion m_OriQuaternion;
    private Transform m_Target, m_SpawnBullet;
    private float m_LastAttackTime;
    private string m_TowerKey;

    public TowerType GetTowerType
    {
        get
        {
            return type;
        }
    }
    
    public void Init(IWeaponRangeModel initWeaponRangeModel, IWeaponModel initWeaponModel, string key)
    {
        m_WeaponRangeModel = initWeaponRangeModel;
        m_WeaponModel = initWeaponModel;
        
        m_TowerKey = key;
        m_OriQuaternion = transform.rotation;
        m_SpawnBullet = transform.Find(TDConstant.GAMEPLAY_TOWER_BULLET_SPAWN);
        m_WeaponModel.GetType(type);
        
        TDTowerWeaponControl.api.onGetLastAttackTime += OnGetLastAttackTime;
    }

    private void OnDestroy()
    {
        /*if (m_WeaponControl != null)
        {
            m_WeaponControl.onGetLastAttackTime -= OnGetLastAttackTime;
        }*/
        TDTowerWeaponControl.api.onGetLastAttackTime -= OnGetLastAttackTime;
    }

    private void OnGetLastAttackTime(string key, float time)
    {
        if (!key.Equals(m_TowerKey)) return;
        
        m_LastAttackTime = time;
    }

    private void Update()
    {
        if(string.IsNullOrEmpty(m_TowerKey)) return;
        
        if (m_Target != null && m_WeaponRangeModel.IsInRange(transform.position, m_Target.position, m_OriQuaternion))
        {
            RotateTowardsTarget();
            TDTowerWeaponControl.api.AttackTarget(m_LastAttackTime, m_WeaponModel, m_Target, m_SpawnBullet, m_TowerKey);
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