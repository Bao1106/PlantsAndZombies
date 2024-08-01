using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class HAGOUIInputFieldComponentView : MonoBehaviour, HAGOUIIComponent
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
	private Text m_txtPlaceholder;
	private Text m_txtUnit;
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

		Debug.Log($"{this.GetType().Name} start init");
		
		m_data = (HAGOUIInputFieldDTO)data;
		m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem = GetComponent<HAGOUIFormItemStatusView>();
		m_ipfContent = transform.Find("IpfContent").GetComponent<InputField>();
		m_txtTitle = transform.Find("IpfContent/TxtTitle").GetComponent<Text>();
		m_txtPlaceholder = transform.Find("IpfContent/Text/Placeholder").GetComponent<Text>();
		m_txtUnit = transform.Find("IpfContent/TxtUnit").GetComponent<Text>();

		m_canvas.interactable = m_isEditMode;
		//
		if(m_data != null) //handle dynamic UI value
        {
			Debug.Log("Init data");

			//set title
			if(m_txtTitle != null) 
			{
				m_txtTitle.text = m_data.Title;
			}
			//set content
			if(m_ipfContent != null)
			{
				SetValue(m_data.Content);
				//
				this.contentType = m_data.ContentType;
				UpdateContentType(this.contentType);
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

			minValue = m_data.MinValue;
			maxValue = m_data.MaxValue;
			minLength = m_data.MinLength;
			maxLength  = m_data.MaxLength;

			unit = m_data.Unit;
		}
		else
		{
			UpdateContentType(this.contentType);
		}

		//set label unit if available
		m_txtUnit.text = unit;

		m_isInitComplete = true;
	}

    private void UpdateContentType(HAGOUIInputFieldContentType type)
	{
		m_ipfContent.contentType = type == HAGOUIInputFieldContentType.Integer ? InputField.ContentType.IntegerNumber : 
									type == HAGOUIInputFieldContentType.Decimal ? InputField.ContentType.DecimalNumber :
										InputField.ContentType.Standard;
	}

	public void SetValue(string content)
	{
		try
		{
			if(m_ipfContent.contentType == InputField.ContentType.IntegerNumber)
			{
				float value = float.Parse(content);
				int roundedValue = Mathf.RoundToInt(value);
				Debug.Log($"Set Value {value} => {roundedValue}");
				content = roundedValue.ToString();
			}
			else
			{
				Debug.Log($"Set Value {content}");
			}
		}
		catch(Exception)
		{
			Debug.Log("[HAGOUIInputFieldComponentView] Failed format string into int");
		}	

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
			m_txtPlaceholder = transform.Find("IpfContent/Text/Placeholder").GetComponent<Text>();
			m_txtTitle = transform.Find("IpfContent/TxtTitle").GetComponent<Text>();
		}

		return new HAGOUIInputFieldDTO(
			id,
			m_txtTitle.text,
			"#" + m_txtPlaceholder.text,
			GetValue(),
			unit,
			contentType,
			GetKeyForm(),
			minValue,
			maxValue,
			minLength,
			maxLength
		);
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
			string valueStr = GetValue();
			switch(contentType)
			{
				case HAGOUIInputFieldContentType.Integer:
					return valueStr.Length > 0 ? (JToken)int.Parse(valueStr) : (JToken)null;

				case HAGOUIInputFieldContentType.Decimal:
					return valueStr.Length > 0 ? (JToken)float.Parse(valueStr) : (JToken)null;

				default:
					return valueStr;
			}
		}
		catch(Exception ex)
		{
			return string.Empty;
		}
	}

	public void Clear()
    {
        SetValue(string.Empty);
    }

    public bool CheckValid()
    {
		if((m_data?.IsRequired ?? false) && string.IsNullOrEmpty(GetValue()))
		{
			return false;
		}

		switch(contentType)
		{
			case HAGOUIInputFieldContentType.Standard:
				if(!IsValidMinLength() || !IsValidMaxLength())
				{
					return false;
				}
				break;

			case HAGOUIInputFieldContentType.Integer:
			case HAGOUIInputFieldContentType.Decimal:
				if(!IsValidMinLength() || !IsValidMaxLength() || !IsValidMinValue() || !IsValidMaxValue())
				{
					return false;
				}
				break;

			default:
				return true;
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

    public bool IsValidMinValue()
    {
		// Debug.Log("IsValidMinValue");
        if(minValue != -1f)
        {
			string valueStr = GetValue();
			if(string.IsNullOrEmpty(valueStr))
			{
				return false;
			}

			try
			{
				switch(contentType)
				{   
					case HAGOUIInputFieldContentType.Integer:
						// Debug.Log("IsValidMinValue " + int.Parse(valueStr) + " >= " +  (int)minValue);
						return int.Parse(valueStr) >= (int)minValue;

					case HAGOUIInputFieldContentType.Decimal:
						// Debug.Log("IsValidMinValue " + float.Parse(valueStr) + " >= " +  (float)minValue);
						return float.Parse(valueStr) >= minValue;

					default:
						return true;
				}
			}
			catch(Exception ex)
			{
				Debug.Log(ex.ToString());
				return false;
			}
        }
        else
        {
            return true;
        }
    }

    public bool IsValidMaxValue()
    {
        if(maxValue != -1f)
        {
			string valueStr = GetValue();
			if(string.IsNullOrEmpty(valueStr))
			{
				return false;
			}

            switch(contentType)
            {   
                case HAGOUIInputFieldContentType.Integer:
                    return int.Parse(valueStr) <= (int)maxValue;

                case HAGOUIInputFieldContentType.Decimal:
                    return float.Parse(valueStr) <= maxValue;

                default:
                    return true;
            }
        }
        else
        {
            return true;
        }
    }

    public Transform GetTransform()
    {
       	return this.transform;
    }
}
