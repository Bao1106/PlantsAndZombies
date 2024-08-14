using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class HAGOUIHelpComponentView : MonoBehaviour, HAGOUIIComponent
{
	public bool isInitByUser = false; //using for dynamic init
    [Space(12)] //blanck space on inspector

	private CanvasGroup m_canvas;
	private HAGOUIFormItemStatusView m_formItem;
	private Text m_txtTitle;
	private Text m_txtContent;

	//param
	public bool IgnoreGetResult { get; set; } = false;
	//
	private HAGOUITextDTO m_data;
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

		m_data = (HAGOUITextDTO)data;
		m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem = GetComponent<HAGOUIFormItemStatusView>();
		m_txtTitle = transform.Find("TxtTitle").GetComponent<Text>();
		m_txtContent = transform.Find("ScrollView/Viewport/Content/Text").GetComponent<Text>();

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
			if(m_txtContent != null)
			{
				SetValue(m_data.Content);
			}
			//set key form response error
			if(m_formItem != null)
			{
				m_formItem.keyItem = this.m_data.KeyForm;
			}
		}

		m_isInitComplete = true;
	}

	public void SetValue(string content)
	{
		m_txtContent.text = content;
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
		return null;
	}
	
	public void Clear()
    {
        //do nothing
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
        return HAGOServiceKey.PARAM_JSON_FORM_HELP_COMPONENT;
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
