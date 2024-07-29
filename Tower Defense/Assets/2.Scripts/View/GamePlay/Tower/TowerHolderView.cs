using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerHolderView : MonoBehaviour
{
    private TMP_Text m_TxtTowerCost;
    private Button m_TowerSelectButton;

    public Button TowerSelectButton
    {
        get
        {
            return m_TowerSelectButton;
        }
    }
    
        
    public void SetupCTowerCost(int cost)
    {
        m_TxtTowerCost.text = $"{cost}$";
    }
        
    public void SetupTowerHolderVariables()
    {
        m_TxtTowerCost = transform.Find(DTConstant.GAMEPLAY_TEXT_COST_TOWER_HOLDER).GetComponent<TMP_Text>();
        m_TowerSelectButton = transform.Find(DTConstant.GAMEPLAY_BUTTON_TOWER_HOLDER).GetComponent<Button>();
    }
}
