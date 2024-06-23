using System;
using System.Collections.Generic;
using Grid_Manager;
using Services.DependencyInjection;
using UnityEngine;
using UnityEngine.Serialization;

namespace TowerFactory
{
    public class TowerSelector : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private List<TowerHolder> towerHolders;
        [Inject] private IGridManager gridManager; 
        
        [Provide] public TowerSelector ProviderTowerSelector() => this;
        
        [HideInInspector] public GameObject currentTower;
        
        private void Start()
        {
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
                    Vector3 gridPosition = gridManager.GetNearestGridPosition(hit.point);
                    currentTower.transform.position = gridPosition;
                }
            }
        }

        private void SetupOnSelectTower()
        {
            foreach (var tower in towerHolders)
            {
                tower.TowerSelectButton.onClick.AddListener(() => OnSelectTower(tower.GetPrefab));
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
    }
}