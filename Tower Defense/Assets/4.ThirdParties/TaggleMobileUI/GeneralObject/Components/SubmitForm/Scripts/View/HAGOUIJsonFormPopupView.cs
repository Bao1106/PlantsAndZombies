using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HAGOUIJsonFormPopupView : MonoBehaviour
{
    private CanvasGroup m_canvas;
    private Transform m_content;
    private ScrollRect m_scrollRect;
    private Button m_btnBack;
    private Text m_txtTitle;
    private Text m_txtError;
    //
    private HAGOUIJsonFormComponentView m_formView;
    //
    private Button m_btnSubmit;

    //param
    private HAGOUIJsonFormComponentDTO m_data;
    private Action<HAGOUIJsonFormDataDTO> m_onCompleteEvent;

    void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        //unregister event
        HAGOSubmitFormControl.Api.ExitEvent -= HidePopup;
    }

    public void Init(HAGOUIJsonFormComponentDTO data, Action<HAGOUIJsonFormDataDTO> onCompleteEvent, bool isShowTitle)
    {
        //find reference
        m_canvas = GetComponent<CanvasGroup>();
        m_content = transform.Find("Content");
        m_btnBack = transform.Find("Content/TopBar/BtnBack").GetComponent<Button>();
        m_scrollRect = transform.Find("Content/Body").GetComponent<ScrollRect>();
        m_txtTitle = transform.Find("Content/Body/Viewport/Content/Info/TxtName").GetComponent<Text>();
        m_formView = transform.Find("Content/Body/Viewport/Content/Info/JsonFormContent").GetComponent<HAGOUIJsonFormComponentView>();
        m_txtError = transform.Find("Content/Body/Viewport/Content/Info/TxtError").GetComponent<Text>();
        m_btnSubmit = transform.Find("Content/Body/Viewport/Content/BtnSubmit").GetComponent<Button>();
        
        //update view
        m_canvas.alpha = 0f;
        SetTextError();

        //add listener
        m_btnBack.onClick.AddListener(BackOnClick);
        m_btnSubmit.onClick.AddListener(SubmitOnClick);

        //register event
        HAGOSubmitFormControl.Api.ExitEvent += HidePopup;

        OnInitFormHandler(data, onCompleteEvent, isShowTitle);
    }

    private void BackOnClick()
    {
        HAGOSubmitFormControl.Api.Exit();
    }

    private void SubmitOnClick()
    {
        //check form valid
        m_formView.GetFormDataResult(formResult => {
            if(formResult.IsSuccess)
            {
                SetTextError();
                HAGOSubmitFormControl.Api.CompleteSubmitForm(formResult.Data);
            }
            else
            {
                return;
            }
        });
    }

    private void OnInitFormHandler(HAGOUIJsonFormComponentDTO data, Action<HAGOUIJsonFormDataDTO> onCompleteEvent, bool isShowTitle)
    {
        m_data = data;
        m_onCompleteEvent = onCompleteEvent;

        //update title
        m_txtTitle.gameObject.SetActive(isShowTitle && !string.IsNullOrEmpty(m_data.Title));
        m_txtTitle.text = m_data.Title;

        //init form submit
        bool showForm = m_data != null;
        m_formView.gameObject.SetActive(showForm);
        if(showForm)
        {
            m_formView.Init(m_data, OnErrorHandler);
        }

        ShowPopup();
    }

    private void ShowPopup()
    {
        HAGOTweenUtils.ShowPopup(m_canvas, m_content);
    }

    private void HidePopup(Action callback)
    {
        HAGOTweenUtils.HidePopup(m_canvas, m_content, callback, false);
    }
    
    private void SetTextError(string error = "")
    {
        m_txtError.gameObject.SetActive(!string.IsNullOrEmpty(error));
        m_txtError.text = I18N.instance.HasKey(error) ? I18N.instance.getValue(error) : error;
    }

    private void OnErrorHandler(RectTransform tfItem)
    {
        SetTextError(HAGOLangConstant.INVALID_FORM_SUBMISSION);
        HAGOTweenUtils.ScrollVerticalTo(m_scrollRect, m_scrollRect.content, tfItem);
    }
}
