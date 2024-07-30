﻿using UnityEngine;

public class TDAreaRangeModel : IWeaponRangeModel
{
    private readonly int m_Range;

    public TDAreaRangeModel(int getRange)
    {
        m_Range = getRange;
    }
        
    public bool IsInRange(Vector3 towerPosition, Vector3 enemyPosition, Quaternion towerRotation)
    {
        float distance = Vector3.Distance(towerPosition, enemyPosition);
        return distance <= m_Range;
    }
}