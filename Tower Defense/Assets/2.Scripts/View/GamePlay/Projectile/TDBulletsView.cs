using UnityEngine;

public class TDBulletsView : MonoBehaviour
{
    public float Damage { get; set; }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyController = other.gameObject.GetComponent<TDEnemyView>();
            enemyController.TakeDamage(Damage);
            TDFlyweightBulletFactoryView.ReturnToPool(this);
        }
    }
}