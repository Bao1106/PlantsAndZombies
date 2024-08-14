using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HAGOCategoryItemView : MonoBehaviour, IPointerUpHandler
{
    private Button m_btnItem;
    private Text m_txtName;
    private RawImage m_rawImg;
    
    //param
    private string m_data;

    public void Init(string category)
    {
        m_data = category;

        //find reference
        m_btnItem = GetComponent<Button>();
        m_txtName = transform.Find("TxtName").GetComponent<Text>();
        m_rawImg = transform.Find("Icon").GetComponent<RawImage>();

        //update value
        m_txtName.text = category;
        m_rawImg.LoadTexture("Icon/" + category);

        //add listener
        m_btnItem.onClick.AddListener(ItemOnClick);
    }

    public void ItemOnClick()
    {
        HAGOEmojiPickerControl.Api.CategorySelected(m_data);
    }

    public void HightLight(bool isHightLight)
    {
        ColorBlock cb = m_btnItem.colors;
        cb.normalColor = HAGOUtils.ParseColorFromString(isHightLight ? HAGOConstant.COLOR_HIGHLIGHT : HAGOConstant.COLOR_TAB_ICON_NORMAL);
        m_btnItem.colors = cb;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
