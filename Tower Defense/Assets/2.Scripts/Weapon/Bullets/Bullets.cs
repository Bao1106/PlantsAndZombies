using Enemy;
using UnityEngine;

namespace Weapon.Bullets
{
    public class Bullets : MonoBehaviour
    {
        public float Damage { get; set; }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemyController = other.gameObject.GetComponent<EnemyController>();
                enemyController.TakeDamage(Damage);
                FlyweightBulletFactory.ReturnToPool(this);
            }
        }
    }
}