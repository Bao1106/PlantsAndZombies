using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class HAGOUIInputFieldMultilineComponentView : MonoBehaviour, HAGOUIIComponent
{
	public bool isInitByUser = false; //using for dynamic init
	public int minLength = -1; // min lenght validate for non standard content type
	public int maxLength = -1; // max lenght validate for non standard content type
    [Space(12)] //blanck space on inspector

	private CanvasGroup m_canvas;
	private HAGOUIFormItemStatusView m_formItem;
	private Text m_txtPlaceholder;
	private Text m_txtTitle;
	private InputField m_ipfContent;

	//param
	[HideInInspector]
	public bool IgnoreGetResult { get; set; } = false;
	//
	private HAGOUIInputFieldDTO m_data;
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

		m_data = (HAGOUIInputFieldDTO)data;
		m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem = GetComponent<HAGOUIFormItemStatusView>();
		m_ipfContent = transform.Find("IpfContent").GetComponent<InputField>();
		m_txtTitle = transform.Find("TxtTitle")?.GetComponent<Text>();
		m_txtPlaceholder = transform.Find("IpfContent/Placeholder").GetComponent<Text>();

		m_canvas.interactable = m_isEditMode;
		//
		if(m_data != null) //handle dynamic UI value
        {
			//set title
			if(m_txtTitle != null) 
			{
				m_txtTitle.text = m_data.Title;
			}
			//set content
			if(m_ipfContent != null)
			{
				SetValue(m_data.Content);
			}
			//set placeholder
			if(m_txtPlaceholder != null)
			{
				m_txtPlaceholder.text = m_data.Placeholder;
			}
			//set key form response error
			if(m_formItem != null)
			{
				m_formItem.keyItem = this.m_data.KeyForm;
			}

			minLength = m_data.MinLength;
			maxLength  = m_data.MaxLength;
		}

		m_isInitComplete = true;
	}

	public void SetValue(string content)
	{
		m_ipfContent.text = content;
	}

	public string GetValue()
	{
		return m_ipfContent.text;
	}

	public string GetID()
    {
        return m_data != null ? m_data.ID : transform.GetSiblingIndex().ToString();
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
		return m_ipfContent.text;
	}

	public void Clear()
    {
        SetValue(string.Empty);
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
		if(m_txtPlaceholder == null || m_ipfContent == null || m_formItem == null)
		{
			m_formItem = GetComponent<HAGOUIFormItemStatusView>();
			m_ipfContent = transform.Find("IpfContent").GetComponent<InputField>();
			m_txtPlaceholder = transform.Find("IpfContent/Placeholder").GetComponent<Text>();
			m_txtTitle = transform.Find("TxtTitle").GetComponent<Text>();
		}

		return new HAGOUIInputFieldDTO(
			id,
			m_txtTitle?.text ?? string.Empty,
			"#" + m_txtPlaceholder.text,
			GetValue(),
			string.Empty,
			HAGOUIInputFieldContentType.Standard,
			GetKeyForm(),
			minLength: minLength,
			maxLength: maxLength
		);
	}

    public string GetFormType()
    {
        return HAGOServiceKey.PARAM_INPUTFIELD_MULTILINE_COMPONENT;
    }

    public bool CheckValid()
    {
        if(m_data != null && m_data.IsRequired && GetValue().Length == 0)
		{
			return false;
		}	

        if(!IsValidMinLength() || !IsValidMaxLength())
		{
			return false;
		}

		m_formItem.ResetError();
		return true;
    }

	public bool IsValidMinLength()
    {
        return minLength != -1 ? GetValue().Length >= minLength : true;
    }

    public bool IsValidMaxLength()
    {
        return maxLength != -1 ? GetValue().Length <= maxLength : true;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
