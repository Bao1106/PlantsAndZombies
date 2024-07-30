using System.Collections.Generic;
using TDEnums;
using Grid_Manager;
using Services.DependencyInjection;
using UnityEngine;

public class TDEnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f, enemyHealth = 1000f;
    [SerializeField] private EnemyAiType aiType;

    [Inject] private IGridManager m_GridManager; 
        
    private List<Vector3> m_PathPositions;
    private int m_CurrentPathIndex;

    public EnemyAiType AiType => aiType;
        
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
        
    public void SetPath(List<IGridCellModel> path)
    {
        Injector.Instance.InjectSingleField(this, typeof(IGridManager));
            
        m_PathPositions = new List<Vector3>();
        foreach (var cell in path)
        {
            Vector3 worldPosition = m_GridManager.GetGrid()[cell.Position.x, cell.Position.y];
            //pathPositions.Add(new Vector3(cell.Position.x, transform.position.y, cell.Position.y));
            m_PathPositions.Add(worldPosition);
        }
        m_CurrentPathIndex = 0;
        //transform.position = pathPositions[0];
        transform.TransformDirection(m_PathPositions[0]);
    }

    private void Update()
    {
        if (m_PathPositions == null) return;
            
        if (m_CurrentPathIndex < m_PathPositions.Count)
        {
            Vector3 targetPosition = m_PathPositions[m_CurrentPathIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            //Missing rotate for enemy

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                m_CurrentPathIndex++;
            }
        }
    }
}