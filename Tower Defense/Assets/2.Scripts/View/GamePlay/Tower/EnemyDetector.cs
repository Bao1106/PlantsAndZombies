using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private TowerWeaponView m_TowerWeaponView;

    private void Start()
    {
        m_TowerWeaponView = GetComponentInParent<TowerWeaponView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_TowerWeaponView.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_TowerWeaponView.SetTarget(null);
        }
    }
}