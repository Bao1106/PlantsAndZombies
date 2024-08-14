using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using UnityEngine;
using UnityEngine.UI;

public class HAGOShowRewardView : MonoBehaviour
{
	private CanvasGroup m_canvas;
	private Transform m_content;
	private RawImage m_rimgIcon;
	private Text m_txtTitle;
	private Text m_txtValue;
	private GameObject m_objButtons;
	private Button m_btnClose;

	//param
	private Queue<HAGORewardShortDTO> m_data;
	private bool m_isAutoTurnOff;
	private Texture m_defaultIcon;

	//const
	private const int CONST_DURATION_DELAY_TURN_OFF = 3;
	
	public void Init(Queue<HAGORewardShortDTO> data, bool isAutoTurnOff)
	{
		m_data = data;
		m_isAutoTurnOff = isAutoTurnOff;

		//find reference
		m_canvas = GetComponent<CanvasGroup>();
		m_content = transform.Find("Content");
		m_rimgIcon = transform.Find("Content/Body/RimgIcon").GetComponent<RawImage>();
		m_txtTitle = transform.Find("Content/Body/Info/Title").GetComponent<Text>();
		m_txtValue = transform.Find("Content/Body/Info/TxtValue").GetComponent<Text>();
		m_objButtons = transform.Find("Content/Body/Buttons").gameObject;
		m_btnClose = transform.Find("Content/Body/Buttons/BtnClose").GetComponent<Button>();

		//update value
		m_defaultIcon = m_rimgIcon.texture;
		m_objButtons.SetActive(!m_isAutoTurnOff);

		//add listener
		m_btnClose.onClick.AddListener(CloseOnClick);

		ShowReward(m_data.Dequeue());
	}

    private void CloseOnClick()
    {
		if(m_data.Count > 0)
		{
			HAGOTweenUtils.HidePopup(
				m_canvas,
				m_content,
				() => {
					ShowReward(m_data.Dequeue());
				},
				false
			);
		}
		else
		{
			HAGOTweenUtils.HidePopup(m_canvas, m_content, HAGOShowRewardControl.Api.CompleteShowReward, false);
		}
    }

	private void ShowReward(HAGORewardShortDTO reward)
	{
		//handle view
		m_txtTitle.text = string.IsNullOrEmpty(reward.Desc) ? I18N.instance.getValue(HAGOLangConstant.YOU_JUST_EARN) : reward.Desc;
		m_txtValue.text = string.Format("{0} x {1}", reward.Name, reward.Quantity);
		//
		if(string.IsNullOrEmpty(reward.Image))
		{
			m_rimgIcon.texture = m_defaultIcon;
		}
		else
		{
			m_rimgIcon.LoadTexture(reward.Image);
		}
		
		HAGOTweenUtils.ShowPopup(m_canvas, m_content, ()=>{
			if(m_isAutoTurnOff)
			{
				Invoke("CloseOnClick", CONST_DURATION_DELAY_TURN_OFF);
			}
		});
	}
}
