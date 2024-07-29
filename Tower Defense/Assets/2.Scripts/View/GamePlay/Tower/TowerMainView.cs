using System.Collections.Generic;
using Grid_Manager;
using Services.DependencyInjection;
using UnityEngine;

namespace TowerFactory
{
    public class TowerMainView : MonoBehaviour, IDependencyProvider, ITowerManipulator
    {
        [SerializeField] private float rotationSpeed = 90f;
        
        [Inject] private IGridManager m_GridManager; 
        
        [Provide] public TowerMainView ProviderTowerSelector() => this;
        
        [HideInInspector] public GameObject currentTower;
        
        private readonly List<TowerHolderView> m_TowerHolders = new List<TowerHolderView>();
        private TowerHolderView m_TowerHolderView0, m_TowerHolderView1, m_TowerHolderView2, m_TowerHolderView3, m_TowerHolderView4;
        
        private int m_CurrentRotationIndex;
        private readonly float[] m_Rotations = { 0f, 90f, 180f, 270f };
        
        private void Start()
        {
            InitTowerHolder();
            SetupOnSelectTower();
        }

        private void Update()
        {
            if (currentTower != null)
            {
                if (Camera.main == null) return;
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    Vector3 gridPosition = m_GridManager.GetNearestGridPosition(hit.point);
                    currentTower.transform.position = gridPosition;
                }
            }
        }

        private void InitTowerHolder()
        {
            m_TowerHolderView0 = transform.Find(DTConstant.GAMEPLAY_TOWER_HOLDER_0).GetComponent<TowerHolderView>();
            m_TowerHolderView0.SetupTowerHolderVariables();
            
            m_TowerHolderView1 = transform.Find(DTConstant.GAMEPLAY_TOWER_HOLDER_1).GetComponent<TowerHolderView>();
            m_TowerHolderView1.SetupTowerHolderVariables();
            
            m_TowerHolderView2 = transform.Find(DTConstant.GAMEPLAY_TOWER_HOLDER_2).GetComponent<TowerHolderView>();
            m_TowerHolderView2.SetupTowerHolderVariables();
            
            m_TowerHolderView3 = transform.Find(DTConstant.GAMEPLAY_TOWER_HOLDER_3).GetComponent<TowerHolderView>();
            m_TowerHolderView3.SetupTowerHolderVariables();
            
            m_TowerHolderView4 = transform.Find(DTConstant.GAMEPLAY_TOWER_HOLDER_4).GetComponent<TowerHolderView>();
            m_TowerHolderView4.SetupTowerHolderVariables();
            
            m_TowerHolders.AddRange(new List<TowerHolderView>
            {
                m_TowerHolderView0, m_TowerHolderView1, m_TowerHolderView2, m_TowerHolderView3, m_TowerHolderView4
            });
        }
        
        private void SetupOnSelectTower()
        {
            foreach (var tower in m_TowerHolders)
            {
                //tower.TowerSelectButton.onClick.AddListener(() => OnSelectTower(tower.GetPrefab));
            }
        }

        private void OnSelectTower(GameObject prefab)
        {
            if (currentTower != null)
            {
                Destroy(currentTower);
            }

            currentTower = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            currentTower.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public void RotateTowerClockwise()
        {
            if (currentTower != null)
            {
                m_CurrentRotationIndex = (m_CurrentRotationIndex + 1) % 4;
                UpdateTowerRotation();
            }
        }

        public void RotateTowerCounterclockwise()
        {
            if (currentTower != null)
            {
                m_CurrentRotationIndex = (m_CurrentRotationIndex - 1 + 4) % 4;
                UpdateTowerRotation();
            }
        }
        
        private void UpdateTowerRotation()
        {
            currentTower.transform.rotation = Quaternion.Euler(0f, m_Rotations[m_CurrentRotationIndex], 0f);
        }

        public void CancelPlacement()
        {
            if (currentTower != null)
            {
                Destroy(currentTower);
                currentTower = null;
            }
        }
    }
}