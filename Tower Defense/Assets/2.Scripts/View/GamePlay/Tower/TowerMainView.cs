using System.Collections.Generic;
using Grid_Manager;
using Services.DependencyInjection;
using UnityEngine;

namespace TowerFactory
{
    public class TowerMainView : MonoBehaviour
    {
        [Inject] private IGridManager m_GridManager;
        
        private readonly List<TowerHolderView> m_TowerHolders = new List<TowerHolderView>();
        private TowerHolderView m_TowerHolderView0, m_TowerHolderView1, m_TowerHolderView2, m_TowerHolderView3, m_TowerHolderView4;
        private GameObject m_CurrentTower;
        
        private int m_CurrentRotationIndex;
        
        private void Start()
        {
            RegistryTowerControlEvents();
            InitTowerHolder();
        }

        private void Update()
        {
            TowerMainControl.api.OnSelectTower(m_CurrentTower, m_GridManager);
            
            if (Input.GetMouseButtonDown(0))
            {
                UserInputControl.api.OnMouseButton0Clicked();
            }
            else if (Input.GetKeyDown(KeyCode.E)) // Press E to rotate clockwise
            {
                UserInputControl.api.OnMouseButtonEClicked();
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Press Q to rotate counterclockwise
            {
                UserInputControl.api.OnMouseButtonQClicked();
            }
            else if (Input.GetMouseButtonDown(1)) // Right click to cancel placement
            {
                UserInputControl.api.OnMouseButton1Clicked();
            }
        }

        private void RegistryTowerControlEvents()
        {
            TowerMainControl.api.onGetTowerName += OnCreateTower;
            TowerMainControl.api.onGetCurrentRotationIndex += OnGetCurrentRotationIndex;
            
            PlaceTowerControl.api.onPlaceTowerSuccess += OnPlaceTowerSuccess;
            
            UserInputControl.api.onMouseButton0Clicked += OnMouseButton0Clicked;
            UserInputControl.api.onMouseButton1Clicked += OnMouseButton1Clicked;
            UserInputControl.api.onMouseButtonEClicked += OnMouseButtonEClicked;
            UserInputControl.api.onMouseButtonQClicked += OnMouseButtonQClicked;
        }

        private void OnDestroy()
        {
            TowerMainControl.api.onGetTowerName -= OnCreateTower;
            TowerMainControl.api.onGetCurrentRotationIndex -= OnGetCurrentRotationIndex;
            
            PlaceTowerControl.api.onPlaceTowerSuccess -= OnPlaceTowerSuccess;
            
            UserInputControl.api.onMouseButton0Clicked -= OnMouseButton0Clicked;
            UserInputControl.api.onMouseButton1Clicked -= OnMouseButton1Clicked;
            UserInputControl.api.onMouseButtonEClicked -= OnMouseButtonEClicked;
            UserInputControl.api.onMouseButtonQClicked -= OnMouseButtonQClicked;
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
            
            SetupOnSelectTower();
        }
        
        private void SetupOnSelectTower()
        {
            foreach (var tower in m_TowerHolders)
            {
                var index = m_TowerHolders.IndexOf(tower);
                tower.towerSelectButton.onClick.AddListener(() => TowerMainControl.api.OnSelectTowerHolder(index));
            }
        }
        
        private void OnCreateTower(string towerName)
        {
            if (m_CurrentTower != null)
            {
                Destroy(m_CurrentTower);
            }

            GameObject prefab = ResourceObject.GetResource<GameObject>(towerName);
            m_CurrentTower = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            m_CurrentTower.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        private void OnGetCurrentRotationIndex(int index)
        {
            m_CurrentRotationIndex = index;
        }
        
        private void OnPlaceTowerSuccess(bool isPlaced)
        {
            if (isPlaced)
            {
                Destroy(m_CurrentTower);
                m_CurrentTower = null;
                PlaceTowerControl.api.onPlaceTowerSuccess?.Invoke(false);
            }
        }
        
        private void OnMouseButtonQClicked(bool isClicked)
        {
            if (isClicked)
            {
                TowerMainControl.api.RotateTowerCounterClockwise(m_CurrentTower, m_CurrentRotationIndex);
                UserInputControl.api.onMouseButtonQClicked(false);
            }
        }
        
        private void OnMouseButtonEClicked(bool isClicked)
        {
            if (isClicked)
            {
                TowerMainControl.api.RotateTowerClockwise(m_CurrentTower, m_CurrentRotationIndex);
                UserInputControl.api.onMouseButtonEClicked(false);
            }
        }
        
        private void OnMouseButton1Clicked(bool isClicked)
        {
            if (isClicked)
            {
                CancelPlacement();
                UserInputControl.api.onMouseButton1Clicked(false);
            }
        }
        
        private void OnMouseButton0Clicked(bool isClicked)
        {
            if (isClicked)
            {
                TowerMainControl.api.OnPlaceTower(m_CurrentTower, m_GridManager);
                UserInputControl.api.onMouseButton0Clicked(false);
            }
        }
        
        public void CancelPlacement()
        {
            if (m_CurrentTower != null)
            {
                Destroy(m_CurrentTower);
                m_CurrentTower = null;
            }
        }
    }
}