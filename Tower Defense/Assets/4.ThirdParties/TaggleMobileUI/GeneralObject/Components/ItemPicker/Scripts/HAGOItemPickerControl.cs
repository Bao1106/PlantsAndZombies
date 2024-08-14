using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOItemPickerControl
{
    private static HAGOItemPickerControl m_api;
    public static HAGOItemPickerControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGOItemPickerControl();
            }
            return m_api;
        }
    }
    
    //event
    public Action<string> ResultCallbackEvent;
    public Action<List<string>> ResultMultipleCallbackEvent;
    
    public void CompleteItemPicker(string id = "")
    {
        if(!string.IsNullOrEmpty(id))
        {
            ResultCallbackEvent?.Invoke(id);
        }
        
        HAGOItemPickerManager.Api.Destroy();
    }

    public void CompleteItemPicker(List<string> ids)
    {
        if(ids != null && ids.Count > 0)
        {
            ResultMultipleCallbackEvent?.Invoke(ids);
        }
        
        HAGOItemPickerManager.Api.Destroy();
    }
}