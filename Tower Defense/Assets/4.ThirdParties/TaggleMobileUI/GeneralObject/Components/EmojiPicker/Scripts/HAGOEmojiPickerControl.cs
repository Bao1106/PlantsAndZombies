using System;

public class HAGOEmojiPickerControl
{
    private static HAGOEmojiPickerControl m_api;
    public static HAGOEmojiPickerControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGOEmojiPickerControl();
            }
            return m_api;
        }
    }
    
    //main event
    public Action<string> ResultCallbackEvent;
    public Action CallBackCompleteLoadDataEvent;


    //event
    public Action<string> OnCategorySelectedEvent;
    public Action<int, bool> OnVisibleCategoryItemEvent; 
    public Action OnCompleteLoadDataEvent;
    public Action<int> OnGetLowestIndexVisibleCategoryEvent;

    public void CompleteEmojiPicker(string content, bool isCacheLoad = false)
    {
        if(!string.IsNullOrEmpty(content))
        {
            ResultCallbackEvent?.Invoke(content);
        }
        
        Exit(isCacheLoad);
    }

    public void Exit(bool isCacheLoad=false)
    {
        HAGOEmojiPickerManager.Api.Exit(isCacheLoad);
    }

    public void CategorySelected(string category)
    {
        OnCategorySelectedEvent?.Invoke(category);
    }

    public void CompleteLoadData()
    {
        OnCompleteLoadDataEvent?.Invoke();
        CallBackCompleteLoadDataEvent?.Invoke();
    }

    public void VisibleCategoryItemView(int indexCategoryView, bool isActive)
    {
        OnVisibleCategoryItemEvent?.Invoke(indexCategoryView, isActive);
    }

    public void GetLowestIndexVisibleCategory(int lowestIndex)
    {
        OnGetLowestIndexVisibleCategoryEvent?.Invoke(lowestIndex);
    }
}