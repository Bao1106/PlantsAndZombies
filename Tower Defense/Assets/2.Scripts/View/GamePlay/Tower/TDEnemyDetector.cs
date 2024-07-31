using UnityEngine;

public class TDEnemyDetector : MonoBehaviour
{
    private TDTowerWeaponView m_TDTowerWeaponView;

    private void Start()
    {
        m_TDTowerWeaponView = GetComponentInParent<TDTowerWeaponView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_TDTowerWeaponView.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_TDTowerWeaponView.SetTarget(null);
        }
    }
}