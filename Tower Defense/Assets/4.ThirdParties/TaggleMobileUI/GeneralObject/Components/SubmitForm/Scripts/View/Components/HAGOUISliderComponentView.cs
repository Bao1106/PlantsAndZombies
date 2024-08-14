using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HAGOUISliderComponentView : MonoBehaviour, HAGOUIIComponent
{
	public bool isInitByUser = false; //using for dynamic init
    [Space(12)] //blanck space on inspector

	private CanvasGroup m_canvas;
	private HAGOUIFormItemStatusView m_formItem;
	private Text m_txtTitle;
	private Transform m_dotsContent;
	private Slider m_slider;
	private Text m_txtSelectedValue;
	//
	private GameObject m_prefDotItem;

	//param
	public bool IgnoreGetResult { get; set; } = false;
	//
	private HAGOUIToggleListDTO m_data;
	private bool m_isInitComplete = false;
	private bool m_isEditMode;
	//
	private Color m_colorDotActive;
	private Color m_colorDotDefault;
	private float m_sliderWitdh;
	
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

		this.m_data = (HAGOUIToggleListDTO)data;
		this.m_isEditMode = isEditMode;

		//find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_formItem = GetComponent<HAGOUIFormItemStatusView>();
		m_slider = transform.Find("Content/SldValue").GetComponent<Slider>();
		m_dotsContent = transform.Find("Content/SldValue/DotsContent");
        m_txtTitle = transform.Find("Title/TxtTitle").GetComponent<Text>();
		m_txtSelectedValue = transform.Find("Title/TxtValue").GetComponent<Text>();
		//
		Image m_imgFill = transform.Find("Content/SldValue/Fill Area/Fill").GetComponent<Image>();
		RawImage rimgIcon = transform.Find("Title/RimgIcon").GetComponent<RawImage>();
		rimgIcon.LoadTexture(
			m_data.Title.Contains("Happy") 		?	"Image/Mood/happy_lvl10001" :
			m_data.Title.Contains("Sad") 		?	"Image/Mood/sad_lvl10001" :
			m_data.Title.Contains("Stressed") 	? 	"Image/Mood/scared_lvl10001" :
											 		"Image/Mood/angry_lvl10001"
		);
		//
		m_prefDotItem = transform.Find("Content/SldValue/DotsContent/DotItem").gameObject;
		m_prefDotItem.SetActive(false);

        //add listener
        m_slider.onValueChanged.AddListener(OnValueChanged);
        
		//update view
		m_canvas.interactable = m_isEditMode;
		//
		m_colorDotActive = m_imgFill.color;
        m_colorDotDefault = m_prefDotItem.transform.Find("Icon").GetComponent<Image>().color;
        m_slider.wholeNumbers = true;
        m_slider.minValue = 0;
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
			StartCoroutine(IEUpdateView());
		}
		else //handle fixed UI value
		{
			InitFixedItems();
		}

		m_isInitComplete = true;
	}

    private void OnValueChanged(float value)
    {
        //set text
        int index = (int)value;

		if(m_data != null)
		{
        	m_txtSelectedValue.text = m_data.Options[index]?.Title ?? string.Empty;
		}

        //update dot color
        foreach(Transform tfDot in m_dotsContent)
        {
            tfDot.Find("Icon").GetComponent<Image>().color = tfDot.GetSiblingIndex() - 1 <= index ? m_colorDotActive : m_colorDotDefault;
        }
    }

    private IEnumerator IEUpdateView()
	{
		yield return new WaitForEndOfFrame();
		m_sliderWitdh = m_slider.GetComponent<RectTransform>().sizeDelta.x;

		//update view
		StartCoroutine(InitDynamicItems());
	}

	private IEnumerator InitDynamicItems()
	{
		//clear old items
		ClearItems();

		int defaultIndex = 0;

		for(int i = 0; i < m_data.Options.Count; i++)
		{
			float anchorPosX = (float)i * (m_sliderWitdh / (m_data.Options.Count - 1));

			GameObject goDot = Instantiate(m_prefDotItem, m_dotsContent);
			goDot.SetActive(true);
			goDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchorPosX, 0f);
			RawImage rimgIcon = goDot.transform.Find("RimgIcon").GetComponent<RawImage>();

			// bool isShowIcon = !string.IsNullOrEmpty(m_data.Options[i].ImgUrl);
			bool isShowIcon = false;

			rimgIcon.gameObject.SetActive(isShowIcon);
			//
			// if(isShowIcon)
			// {
				// rimgIcon.LoadTexture(m_data.Options[i].ImgUrl);
			// }

			if(m_data.Options[i].DefaulValue)
			{
				defaultIndex = i;
			}
		}

		m_slider.maxValue = m_data.Options.Count - 1;

		yield return new WaitForEndOfFrame();
		m_slider.value = defaultIndex;
	}

	private void InitFixedItems()
	{
		foreach(Transform tf in m_dotsContent)
		{
			HAGOUIToggleListItemView itemView = tf.GetComponent<HAGOUIToggleListItemView>();
			itemView.Init(null, m_isEditMode);
		}
	}

	private void ClearItems()
	{
		foreach(Transform tf in m_dotsContent)
		{
			if(tf.gameObject == m_prefDotItem)
			{
				continue;
			}

			if(Application.isPlaying)
			{
				Destroy(tf.gameObject);
			}
			else
			{
				DestroyImmediate(tf.gameObject);
			}
		}
	}

	public void ActiveError()
    {
        m_formItem.ActiveError();
    }

	public void ResetError()
	{
		m_formItem.ResetError();
	}

	public int GetValue()
	{
		try
		{
			return int.Parse(m_data.Options[(int)m_slider.value].ID);
		}
		catch(Exception ex)
		{
			return 0;
		}
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
		return GetValue();
	}

	public void Clear()
    {
        //do nothing
    }

	public object ExportView(string id)
	{		
		return null;
	}

    public string GetFormType()
    {
        return HAGOServiceKey.PARAM_TOGGLE_LIST_COMPONENT;
    }

	public void SetValue(string content)
	{
		try
		{
			// //<id, option>
			// Dictionary<string, bool> data = new Dictionary<string, bool>();

			// JArray ja = JsonConvert.DeserializeObject<JArray>(content);
			// foreach(JToken jt in ja)
			// {
			// 	JObject jo = (JObject)jt;
			// 	string id = jo.Value<string>(HAGOServiceKey.PARAM_ID);
			// 	bool value = jo.Value<bool>(HAGOServiceKey.PARAM_VALUE);

			// 	if(!data.ContainsKey(id))
			// 	{
			// 		data.Add(id, value);
			// 	}
			// }

			// //update view
			// foreach(HAGOUIToggleListItemView itemView in m_dotsContent.GetComponentsInChildren<HAGOUIToggleListItemView>())
			// {
			// 	if(data.ContainsKey(itemView.GetID()))
			// 	{
			// 		itemView.SetValue(data[itemView.GetID()]);
			// 	}
			// }
		}
		catch(Exception ex)
		{
			Debug.Log("[HASliderComponentView] Cannot parse value: " + ex.ToString());
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