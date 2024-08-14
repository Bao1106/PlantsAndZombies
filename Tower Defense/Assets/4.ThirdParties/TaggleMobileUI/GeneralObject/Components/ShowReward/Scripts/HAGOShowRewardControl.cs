using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOShowRewardControl
{
    private static HAGOShowRewardControl m_api;
    public static HAGOShowRewardControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGOShowRewardControl();
            }
            return m_api;
        }
    }
    
    //event
    public Action ResultCallbackEvent;
    
    public void CompleteShowReward()
    {
        ResultCallbackEvent?.Invoke();
        HAGOShowRewardManager.Api.Destroy();
    }
}