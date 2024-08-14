using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAGOSubmitJsonFormManager : MonoBehaviour
{
    private static HAGOSubmitJsonFormManager m_api;
    public static HAGOSubmitJsonFormManager Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = Instantiate(Resources.Load<GameObject>(HAGOConstant.PREFAB_SUBMIT_JSON_FORMS)).GetComponent<HAGOSubmitJsonFormManager>();
            }
            return m_api;
        }
    }

    private HAGOUIJsonFormPopupView m_view;

    public void Init(HAGOUIJsonFormComponentDTO data, Action<HAGOUIJsonFormDataDTO> onCompleteEvent, bool isShowTitle = true)
    {
        HAGOSubmitFormControl.Api.ResultCallbackEvent = onCompleteEvent;
        
        //init view
        m_view = transform.Find("Canvas").GetComponent<HAGOUIJsonFormPopupView>();
        m_view?.Init(data, onCompleteEvent, isShowTitle);
    }

    public void Destroy()
    {
        //unregister event
        m_view?.Destroy();

        Destroy(this.gameObject);
    }
}
