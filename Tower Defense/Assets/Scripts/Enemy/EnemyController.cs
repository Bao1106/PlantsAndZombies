using System.Collections.Generic;
using Enums;
using Grid_Manager;
using Interfaces.Grid;
using Services.DependencyInjection;
using UnityEngine;
using Weapon.Bullets;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f, enemyHealth = 1000f;
        [SerializeField] private EnemyAiType aiType;

        [Inject] private IGridManager gridManager; 
        
        private List<Vector3> pathPositions;
        private int currentPathIndex;

        public EnemyAiType AiType => aiType;
        
        public void TakeDamage(float damage)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        public void SetPath(List<IGridCell> path)
        {
            Injector.Instance.InjectSingleField(this, typeof(IGridManager));
            
            pathPositions = new List<Vector3>();
            foreach (var cell in path)
            {
                Vector3 worldPosition = gridManager.GetGrid()[cell.Position.x, cell.Position.y];
                //pathPositions.Add(new Vector3(cell.Position.x, transform.position.y, cell.Position.y));
                pathPositions.Add(worldPosition);
            }
            currentPathIndex = 0;
            //transform.position = pathPositions[0];
            transform.TransformDirection(pathPositions[0]);
        }

        private void Update()
        {
            if (pathPositions == null) return;
            
            if (currentPathIndex < pathPositions.Count)
            {
                Vector3 targetPosition = pathPositions[currentPathIndex];
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                //Missing rotate for enemy

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    currentPathIndex++;
                }
            }
        }
    }
}