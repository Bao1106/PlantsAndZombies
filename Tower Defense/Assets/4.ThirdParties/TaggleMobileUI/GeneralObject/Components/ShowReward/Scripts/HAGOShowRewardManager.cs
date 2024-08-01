using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAGOShowRewardManager : MonoBehaviour
{
	private static HAGOShowRewardManager m_api;
    public static HAGOShowRewardManager Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = Instantiate(Resources.Load<GameObject>(HAGOConstant.PREFAB_SHOW_REWARD)).GetComponent<HAGOShowRewardManager>();
            }
            return m_api;
        }
    }

	public void Init(List<HAGORewardShortDTO> rewards, bool isAutoTurnOff = false, Action onCloseEvent = null)
	{
        Queue<HAGORewardShortDTO> data = new Queue<HAGORewardShortDTO>(rewards);
        HAGOShowRewardControl.Api.ResultCallbackEvent = onCloseEvent;
        
        //init view
        HAGOShowRewardView view = transform.Find("Canvas").GetComponent<HAGOShowRewardView>();
        view.Init(data, isAutoTurnOff);
	}

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}