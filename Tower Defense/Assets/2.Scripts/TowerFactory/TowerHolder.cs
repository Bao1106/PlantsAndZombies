using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFactory
{
    public class TowerHolder : MonoBehaviour
    {
        [SerializeField] private int towerCost;
        [SerializeField] private TMP_Text txtTowerCost;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Button towerSelectButton;

        public Button TowerSelectButton => towerSelectButton;
        public GameObject GetPrefab => prefab;
        
        private void Start()
        {
            SetupCostText();
        }

        private void SetupCostText()
        {
            txtTowerCost.text = $"{towerCost}$";
        }
    }
}
