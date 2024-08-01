using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

public class HAGOUISelectTypeComponentView : MonoBehaviour, HAGOUIIComponent
{
	private CanvasGroup m_canvas;
	private HAGOUIFormItemStatusView m_formItem;
	private Text m_txtTitle;
	private Button m_btnAdd;
	private Button m_btnEmptyContent;
	private Transform m_content;
	private GameObject  m_prefItem;

	//param
	public bool IgnoreGetResult { get; set; } = false;
	//
	private HAGOUIToggleListDTO m_data;
	private Dictionary<string, HAGOItemPickerDTO> m_pickerItems = new Dictionary<string, HAGOItemPickerDTO>();
	private List<string> m_values = new List<string>();
	private bool m_isInitComplete = false;
	private bool m_isEditMode;

	public void Init(object data, bool isEditMode)
	{
		if(m_isInitComplete)
        {
            Debug.LogError(this.GetType().Name + " already init! Please set isInitByUser = false in inspector if init via script.");
            return;
        }

		this.m_data = (HAGOUIToggleListDTO)data;
		this.m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem = GetComponent<HAGOUIFormItemStatusView>();
        m_txtTitle = transform.Find("Control/Info/TxtTitle").GetComponent<Text>();
		m_btnAdd = transform.Find("Control/Info/BtnAdd").GetComponent<Button>();
		m_btnEmptyContent = transform.Find("Control/EmptyContent").GetComponent<Button>();
		m_content = transform.Find("Control/Content");
		//
        m_prefItem = transform.Find("Control/Content/SelectedItem").gameObject;
		m_prefItem.SetActive(false);
		
		m_canvas.interactable = m_isEditMode;
		m_btnAdd.gameObject.SetActive(m_isEditMode);
		//
		if(this.m_data != null) //handle dynamic UI value
        {
			//set title
            if(m_txtTitle != null)
            {
                m_txtTitle.text = this.m_data.Title;
            }
			//set key form response error
			if(m_formItem != null)
			{
				m_formItem.keyItem = this.m_data.KeyForm;
			}

			//set item pickers
			m_pickerItems = new Dictionary<string, HAGOItemPickerDTO>();

			foreach(HAGOUIToggleOptionDTO option in m_data.Options)
			{
				HAGOItemPickerDTO itemDTO = new HAGOItemPickerDTO(option.ID, option.Title, string.Empty, string.Empty);
				m_pickerItems.Add(itemDTO.Id, itemDTO);
			}
		}

		//add listener
		m_btnAdd.onClick.AddListener(ItemOnClick);
		m_btnEmptyContent.onClick.AddListener(ItemOnClick);

		m_isInitComplete = true;
	}

    private void ItemOnClick()
    {
		List<HAGOItemPickerDTO> items = m_pickerItems.Values.Where(x => !m_values.Contains(x.Id)).ToList();

		if(items.Count == 0)
		{
			return;
		}

		if(m_data.IsSelectMultiple)
		{
			HAGOItemPickerManager.Api.InitPickMultiple(items, selectedIds => {
				SetOption(selectedIds);
			});
		}
		else
		{
			HAGOItemPickerManager.Api.Init(items, selectedId => {
				SetOption(selectedId);
			});
		}
    }

	public void SetOption(string itemId)
	{
		SetOption(new List<string>(){ itemId });
	}

	public void SetOption(List<string> itemIds)
	{
		foreach (string newId in itemIds)
		{
			if(m_values.Contains(newId))
			{
				itemIds.Remove(newId);
			}
		}

		m_values.AddRange(itemIds);

		foreach (string itemId in itemIds)
		{
        	HAGOUIToggleOptionDTO option = m_data.Options.Where(x => x.ID == itemId).FirstOrDefault();

			if(option != null)
			{
				GameObject goItem = Instantiate(m_prefItem, m_content);
				goItem.SetActive(true);

				goItem.transform.Find("SelectedItemLabel").GetComponent<Text>().text = option.Title;
				goItem.transform.Find("BtnRemove").GetComponent<Button>().onClick.AddListener(() => {
					Destroy(goItem);
					RemoveOption(option.ID);
				});
			}
		}

		StartCoroutine(IEUpdateEmptyContent());
	}

	public void Clear()
    {
        foreach(Transform tf in m_content)
		{
			if(tf.gameObject != m_prefItem)
			{
				Destroy(tf.gameObject);
			}
		}
    }

	private IEnumerator IEUpdateEmptyContent()
	{
		yield return new WaitForEndOfFrame();
		m_btnEmptyContent.gameObject.SetActive(m_values.Count == 0);
	}

	private void RemoveOption(string id)
	{
		if(m_values.Contains(id))
		{
			m_values.Remove(id);
		}

		StartCoroutine(IEUpdateEmptyContent());
	}

	public void ActiveError()
    {
        m_formItem.ActiveError();
    }

	public void ResetError()
	{
		m_formItem.ResetError();
	}

	public List<string> GetValue()
	{
		return m_values;
	}

	public string GetKeyForm()
	{
		if(m_formItem == null)
		{
			m_formItem = GetComponent<HAGOUIFormItemStatusView>();
		}

		return m_formItem != null ? m_formItem.keyItem : string.Empty;
	}

	public JToken GetJsonValue()
	{
		if(m_data.IsSelectMultiple)
		{
			return JArray.FromObject(GetValue());
		}
		else
		{
			return GetValue().FirstOrDefault();
		}
	}

	public string GetID()
    {
        return m_data != null ? m_data.ID : transform.GetSiblingIndex().ToString();
    }

	public object ExportView(string id)
	{
		//unused flow export form
		return null;
	}

    public string GetFormType()
    {
        return HAGOServiceKey.PARAM_DROPDOWN_COMPONENT;
    }

	public void SetValue(string value)
    {
		//TODO: handle later
		return;
    }

    public bool CheckValid()
    {
        return m_data.IsRequired ? m_values.Count > 0 : true;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
