using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAGOItemPickerView : MonoBehaviour
{
	private CanvasGroup m_canvas;
	private Transform m_content;
	private Text m_txtTitle;
	private Button m_btnClose;
	private Transform m_tfItemContent;
	private GameObject m_prefItem;
	private Button m_btnConfirm;

	//param
	private List<HAGOItemPickerDTO> m_data;
	private bool m_isMultiple;
	private List<string> m_listPickMultipleIds = new List<string>();
	
	public void Init(string title, List<HAGOItemPickerDTO> data, bool isMultiple)
	{
		m_isMultiple = isMultiple;

		//find reference
		m_canvas = GetComponent<CanvasGroup>();
		m_content = transform.Find("Content");
		m_txtTitle = transform.Find("Content/Body/Header/TxtTitle").GetComponent<Text>();
		m_btnClose = transform.Find("Content/Body/Header/BtnClose").GetComponent<Button>();
		m_tfItemContent = transform.Find("Content/Body/ScrItem/Viewport/Content");
		m_btnConfirm = transform.Find("Content/Body/BtnConfirm").GetComponent<Button>();
		//
		m_prefItem = transform.Find("Content/Body/ScrItem/Viewport/Content/Item").gameObject;
		m_prefItem.SetActive(false);

		//add listener
		m_btnClose.onClick.AddListener(CloseOnClick);
		m_btnConfirm.onClick.AddListener(ConfirmOnClick);

		InitView(title, data);
	}

    private void ConfirmOnClick()
    {
        CloseView(m_listPickMultipleIds);
    }

    private void CloseOnClick()
    {
		CloseView();
    }

	private void InitView(string title, List<HAGOItemPickerDTO> data)
	{
		m_data = data;

		m_btnConfirm.gameObject.SetActive(m_isMultiple);

		m_txtTitle.text = HAGOUtils.GetLanguageValue(title);

		//handle view
		foreach(HAGOItemPickerDTO item in m_data)
		{
			GameObject goItem = Instantiate(m_prefItem, m_tfItemContent);
			goItem.gameObject.SetActive(true);

			RawImage rimgIcon = goItem.transform.Find("ItemIcon").GetComponent<RawImage>();
			rimgIcon.gameObject.SetActive(!string.IsNullOrEmpty(item.Icon));
			rimgIcon.LoadTexture(item.Icon);

			goItem.transform.Find("ItemLabel").GetComponent<Text>().text = HAGOUtils.GetLanguageValue(item.Name);
			goItem.transform.Find("ItemDesc").GetComponent<Text>().text = item.Desc;
			goItem.transform.Find("Selected/Icon").gameObject.SetActive(item.IsSelected);

			if(m_isMultiple && item.IsSelected)
			{
				OnItemOnClick(goItem, item.Id);
			}

			goItem.GetComponent<Button>().onClick.AddListener(() => OnItemOnClick(goItem, item.Id));
		}
		
		ShowView();
	}

	private void OnItemOnClick(GameObject goItem, string id)
	{
		if(m_isMultiple)
		{
			GameObject iconSelect = goItem.transform.Find("Selected/Icon").gameObject;

			bool isSelected = iconSelect.activeSelf;
			if(iconSelect.activeSelf)
			{
				m_listPickMultipleIds.Remove(id);
			}
			else
			{
				m_listPickMultipleIds.Add(id);
			}

			iconSelect.gameObject.SetActive(!iconSelect.activeSelf);
		}
		else
		{
			CloseView(id);
		}
	}

	private void ShowView()
	{
		HAGOTweenUtils.ShowPopup(m_canvas, m_content);
	}

	private void CloseView(string id = "")
	{
		HAGOTweenUtils.HidePopup(m_canvas, m_content, () => HAGOItemPickerControl.Api.CompleteItemPicker(id), false);
	}

	private void CloseView(List<string> ids)
	{
		HAGOTweenUtils.HidePopup(m_canvas, m_content, () => HAGOItemPickerControl.Api.CompleteItemPicker(ids), false);
	}
}