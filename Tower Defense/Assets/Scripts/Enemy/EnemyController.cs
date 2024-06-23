using System.Collections.Generic;
using Enums;
using Interfaces.Grid;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private EnemyAiType aiType;
        
        private List<Vector3> pathPositions;
        private int currentPathIndex;

        public EnemyAiType AiType => aiType;
        
        public void SetPath(List<IGridCell> path)
        {
            pathPositions = new List<Vector3>();
            foreach (var cell in path)
            {
                pathPositions.Add(new Vector3(cell.Position.x, transform.position.y, cell.Position.y));
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

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    currentPathIndex++;
                }
            }
        }
    }
}