using UnityEngine;

public class BulletsView : MonoBehaviour
{
    public float Damage { get; set; }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyController = other.gameObject.GetComponent<EnemyController>();
            enemyController.TakeDamage(Damage);
            FlyweightBulletFactoryView.ReturnToPool(this);
        }
    }
}