using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class HAGOUITimeComponentView : MonoBehaviour, HAGOUIIComponent
{
	public bool isInitByUser = false; //using for dynamic init
    [Space(12)] //blanck space on inspector

	private CanvasGroup m_canvas;
	private Button m_btnControl;
	private HAGOUIFormItemStatusView m_formItem;
	private Text m_txtTitle;
	private Text m_txtValue;

	//param
	public bool IgnoreGetResult { get; set; } = false;
	//
	private HAGOUIDateTimeComponentDTO m_data;
	private TimeSpan m_value;
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

		this.m_data = (HAGOUIDateTimeComponentDTO)data;
		this.m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem =  GetComponent<HAGOUIFormItemStatusView>();
		m_btnControl = transform.Find("BtnTime").GetComponent<Button>();
		m_txtValue = transform.Find("BtnTime/Text").GetComponent<Text>();
        m_txtTitle = transform.Find("BtnTime/TxtTitle").GetComponent<Text>();
		
		m_canvas.interactable = m_isEditMode;
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
			//
			SetTime(m_data.Value.TimeOfDay);
		}
		else //handle fixed UI value
		{
			SetTime(m_data.IsDurationPicker ? DateTime.MinValue.TimeOfDay : DateTime.Now.TimeOfDay);
		}

		//add listener
		m_btnControl.onClick.AddListener(TimeOnClick);

		m_isInitComplete = true;
	}

	private void TimeOnClick()
	{
		HAGODateTimePickerManager.Api.InitTimePicker(SetTime, m_data.IsDurationPicker ? HAGOTimePickerType.Duration : HAGOTimePickerType.Format12Hours);
	}

	public void SetTime(TimeSpan time)
	{
		if (time == null)
		{
			return;
		}

		m_value = time;
		SetTextValue();
	}

	private void SetTextValue()
	{
		m_txtValue.text =  m_data.IsDurationPicker ? DateTime.MinValue.Add(m_value).ToString(HAGOConstant.FORMAT_TIME_DURATION) : DateTime.Today.Add(m_value).ToString(HAGOConstant.FORMAT_TIME_12_HOURS);
	}

	public void ActiveError()
    {
        m_formItem.ActiveError();
    }

	public void ResetError()
	{
		m_formItem.ResetError();
	}

	public TimeSpan GetValue()
	{
		return m_value;
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
		return new DateTime().Add(m_value).ToUniversalTime().ToISOStringDatetime();
	}

	public void Clear()
    {
        SetTime(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0));
    }

	public object ExportView(string id)
	{
		if(m_txtTitle == null)
        {
            m_txtTitle = transform.Find("BtnTime/TxtTitle").GetComponent<Text>();
        }

		if(m_formItem == null)
        {
            m_formItem = GetComponent<HAGOUIFormItemStatusView>();
        }

		return new HAGOUIDateTimeComponentDTO(
			id,
			m_txtTitle.text,
			GetKeyForm(),
			DateTime.MinValue.Add(GetValue())
		);
	}

    public string GetFormType()
    {
        return HAGOServiceKey.PARAM_TIME_COMPONENT;
    }

	public void SetValue(string content)
	{
		try
		{
			long TotalSeconds = long.Parse(content);
			SetTime(TimeSpan.FromSeconds(TotalSeconds));
		}
		catch(Exception ex)
		{
			Debug.Log("[HATimeComponentView] Cannot parse value: " + ex.ToString());
		}
	}

    public bool CheckValid()
    {
        return true;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
