using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Linq;

public class HAGOUIInputFieldListComponentView : MonoBehaviour, HAGOUIIComponent
{
	public bool isInitByUser = false; //using for dynamic init
	public string unit = string.Empty; // unit if vsm data
	[SerializeField]
	public HAGOUIInputFieldContentType contentType; // content type inputfield
	public float minValue = -1; // min value validate for non standard content type
	public float maxValue = -1; // max value validate for non standard content type
	public int minLength = -1; // min lenght validate for non standard content type
	public int maxLength = -1; // max lenght validate for non standard content type
    [Space(12)] //blanck space on inspector

	private CanvasGroup m_canvas;
	private HAGOUIFormItemStatusView m_formItem;
	private Text m_txtTitle;
	private Button m_btnAdd;
	private Transform m_content;
	private GameObject m_prefItem;
	
	//param
	public bool IgnoreGetResult { get; set; } = false;
	//
	private HAGOUIInputFieldListDTO m_data;
	private bool m_isInitComplete = false;
	private bool m_isEditMode;

	void Start()
    {
        if(!isInitByUser)
        {
			Init(null, true);
        }
    }
	
	public void Init(object data, bool isEditMode)
	{
		if(m_isInitComplete)
        {
            Debug.LogError(this.GetType().Name + " already init! Please set isInitByUser = false in inspector if init via script.");
            return;
        }

		Debug.Log($"{this.GetType().Name} start init");
		
		m_data = (HAGOUIInputFieldListDTO)data;
		m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem = GetComponent<HAGOUIFormItemStatusView>();
		m_txtTitle = transform.Find("Info/TxtTitle").GetComponent<Text>();
		m_btnAdd = transform.Find("Info/BtnAdd").GetComponent<Button>();
		m_content = transform.Find("Content");
		//
		m_prefItem = transform.Find("Content/Item").gameObject;
		m_prefItem.SetActive(false);

		m_canvas.interactable = m_isEditMode;
		m_btnAdd.gameObject.SetActive(m_isEditMode);
		//
		if(m_data != null) //handle dynamic UI value
        {
			Debug.Log("Init data");

			//set title
			if(m_txtTitle != null) 
			{
				m_txtTitle.text = m_data.Title;
			}
			//set key form response error
			if(m_formItem != null)
			{
				m_formItem.keyItem = this.m_data.KeyForm;
			}

			minValue = m_data.MinValue;
			maxValue = m_data.MaxValue;
			minLength = m_data.MinLength;
			maxLength  = m_data.MaxLength;

			unit = m_data.Unit;

			if(m_data.Contents.Count > 0)
			{
				foreach (string content in m_data.Contents)
				{
					CreateItem(content);
				}
			}
			else
			{
				if(m_isEditMode)
				{
					CreateItem(string.Empty);
				}
			}
		}
		else
		{
			CreateItem(string.Empty);
		}

		//add listener
		m_btnAdd.onClick.AddListener(AddItemOnClick);

		m_isInitComplete = true;
	}

	private void AddItemOnClick()
	{
		CreateItem(string.Empty);
	}

	private void CreateItem(string content)
	{
		string tmpId = DateTime.Now.Ticks.ToString();
		HAGOUIInputFieldDTO data = new HAGOUIInputFieldDTO(
			tmpId, string.Empty, m_data?.Placeholder ?? string.Empty, content, m_data?.Unit ?? string.Empty,
			m_data?.ContentType ?? contentType, (m_data?.KeyForm ?? "#") + tmpId,
        	m_data?.MinValue ?? minValue, m_data?.MaxValue ?? maxValue, m_data?.MinLength ?? minLength, m_data?.MaxLength ?? maxLength, m_data?.IsRequired ?? true
		);

		GameObject go = Instantiate(m_prefItem, m_content);
		go.SetActive(true);

		HAGOUIInputFieldMultilineComponentView itemView = go.GetComponent<HAGOUIInputFieldMultilineComponentView>();
		itemView.IgnoreGetResult = true;
		itemView.Init(data, m_isEditMode);

		go.transform.Find("BtnRemove")?.GetComponent<Button>().onClick.AddListener(() => {
			Destroy(go);
		});
	}

	public void SetValue(string content)
	{
		//unused flow
	}

	public string GetID()
    {
        return m_data != null ? m_data.ID : transform.GetSiblingIndex().ToString();
    }

	public void ActiveError()
    {
        m_formItem.ActiveError();
    }
	
	public void ResetError()
	{
		m_formItem.ResetError();
	}

	public object ExportView(string id)
	{
		return null;
	}

    public string GetFormType()
    {
        return HAGOServiceKey.PARAM_INPUTFIELD_COMPONENT;
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
		try
		{
			HAGOUIInputFieldMultilineComponentView[] items = m_content.GetComponentsInChildren<HAGOUIInputFieldMultilineComponentView>();
			return JArray.FromObject(items.Select(x => x.GetJsonValue()).ToList());
		}
		catch(Exception ex)
		{
			return string.Empty;
		}
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

    public bool CheckValid()
    {
		HAGOUIInputFieldMultilineComponentView[] items = m_content.GetComponentsInChildren<HAGOUIInputFieldMultilineComponentView>();
		return items.All(x => x.CheckValid());
    }

    public Transform GetTransform()
    {
       	return this.transform;
    }
}
