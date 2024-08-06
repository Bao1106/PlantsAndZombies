﻿using TDEnums;
using UnityEngine;

public class TDTowerWeaponView : MonoBehaviour
{
    [SerializeField] private TowerType type;
    
    private IWeaponRangeDTO m_WeaponRangeDTO;
    //private IWeaponDTO m_WeaponDTO;
    private Quaternion m_OriQuaternion;
    private Transform m_Target, m_PosSpawnBullet;
    private float m_LastAttackTime;
    private string m_TowerKey;

    public TowerType towerType
    {
        get
        {
            return type;
        }
    }
    
    public void Init(IWeaponRangeDTO initWeaponRangeDTO, string key)
    {
        m_WeaponRangeDTO = initWeaponRangeDTO;
        //m_WeaponDTO = initWeaponDTO;
        
        m_TowerKey = key;
        m_OriQuaternion = transform.rotation;
        m_PosSpawnBullet = transform.Find(TDConstant.GAMEPLAY_TOWER_BULLET_SPAWN);
        //m_WeaponDTO.SetupType(type);
        
        TDTowerWeaponControl.api.onGetLastAttackTime += OnGetLastAttackTime;
    }

    private void OnDestroy()
    {
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
        
        if (m_Target != null && m_WeaponRangeDTO.IsInRange(transform.position, m_Target.position, m_OriQuaternion))
        {
            RotateTowardsTarget();
            TDTowerWeaponControl.api
                .AttackTarget(m_LastAttackTime, m_Target, m_PosSpawnBullet, m_TowerKey, towerType);
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