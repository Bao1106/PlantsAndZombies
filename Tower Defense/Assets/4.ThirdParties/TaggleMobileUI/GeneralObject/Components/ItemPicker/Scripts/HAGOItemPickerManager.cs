using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAGOItemPickerManager : MonoBehaviour
{
	private static HAGOItemPickerManager m_api;
    public static HAGOItemPickerManager Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = Instantiate(Resources.Load<GameObject>(HAGOConstant.PREFAB_ITEM_PICKER)).GetComponent<HAGOItemPickerManager>();
            }
            return m_api;
        }
    }

	public void Init(List<HAGOItemPickerDTO> items, Action<string> onItemSelectedEvent = null, string title = "")
	{
        List<HAGOItemPickerDTO> data = new List<HAGOItemPickerDTO>(items);
        HAGOItemPickerControl.Api.ResultCallbackEvent = onItemSelectedEvent;
        
        //init view
        HAGOItemPickerView view = transform.Find("Canvas").GetComponent<HAGOItemPickerView>();
        view.Init(title, data, false);
	}

	public void InitPickMultiple(List<HAGOItemPickerDTO> items, Action<List<string>> onItemSelectedEvent = null, string title = "")
	{
        List<HAGOItemPickerDTO> data = new List<HAGOItemPickerDTO>(items);
        HAGOItemPickerControl.Api.ResultMultipleCallbackEvent = onItemSelectedEvent;
        
        //init view
        HAGOItemPickerView view = transform.Find("Canvas").GetComponent<HAGOItemPickerView>();
        view.Init(title, data, true);
	}

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}