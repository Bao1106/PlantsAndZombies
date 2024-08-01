using System;
using System.Collections;
using System.Collections.Generic;
using Kyub.EmojiSearch.UI;
using UnityEngine;
using UnityEngine.UI;

public class HAGOEmojiPickerView : MonoBehaviour
{
	private CanvasGroup m_canvas;
	private Transform m_content;
	private TMP_EmojiTextUGUI m_tmpOuput;
	private Button m_btnClearOutput;
	private CanvasGroup m_cvgButtons;
	private Button m_btnClose;
	private Button m_btnDone;
	private ScrollRect m_scrTitleCategory;
	private HAGOScrollOcclusion m_scrListEMojiOcclusion;
	private ScrollRect m_scrListEmoji;
	private RectTransform m_rectScrCategory;
	private RectTransform m_rectCategoryContent;
	private RectTransform m_rectEmojiContent;
	private GameObject m_prefCategoryItem;
	private GameObject m_prefCategoryContainerItem;
	private GameObject m_prefEmojiItem;

	//param
	private bool m_isCacheLoadData;
	private Dictionary<string, RectTransform> m_dictContainerEmoji = new Dictionary<string, RectTransform>();
	private string m_result;
	private GameObject m_cacheEmojiObj;
	private HAGOCategoryItemView m_firstCategoryItemView;
	private List<HAGOCategoryItemView> m_listCategoryItemView = new List<HAGOCategoryItemView>();
	private bool m_isScrollingSeletectedCategory;

	WaitForSeconds m_delayCreateCategory = new WaitForSeconds(0.2f);

	void OnDestroy()
	{
        Destroy();
	}

	public void Destroy()
	{
		//unregister event
        HAGOEmojiPickerControl.Api.OnCategorySelectedEvent -= OnCategorySelectedHandler;
		HAGOEmojiPickerControl.Api.OnVisibleCategoryItemEvent -= OnVisibleCategoryItemHandler;
		HAGOEmojiPickerControl.Api.OnGetLowestIndexVisibleCategoryEvent -= OnGetLowestIndexVisibleCategoryHandler;

	}

    public void Init(Dictionary<string, List<string>> data, bool isCacheLoad = false)
	{
		//find reference
		m_canvas = GetComponent<CanvasGroup>();
		m_content = transform.Find("Content");
		m_tmpOuput = transform.Find("Content/Body/Output/Viewport/TxtOutput").GetComponent<TMP_EmojiTextUGUI>();
		m_btnClearOutput = transform.Find("Content/Body/Output/BtnClear").GetComponent<Button>();
		m_cvgButtons = transform.Find("Content/Body/Buttons").GetComponent<CanvasGroup>();
		m_btnClose = transform.Find("Content/Body/Buttons/BtnClose").GetComponent<Button>();
		m_btnDone = transform.Find("Content/Body/Buttons/BtnDone").GetComponent<Button>();

		m_scrTitleCategory = transform.Find("Content/Body/Category").GetComponent<ScrollRect>();
		m_rectScrCategory = transform.Find("Content/Body/Category").GetComponent<RectTransform>();
		m_rectCategoryContent = transform.Find("Content/Body/Category/Viewport/Content").GetComponent<RectTransform>();
		m_scrListEmoji = transform.Find("Content/Body/ListEmoji").GetComponent<ScrollRect>();
		m_scrListEMojiOcclusion = transform.Find("Content/Body/ListEmoji").GetComponent<HAGOScrollOcclusion>();
		m_rectEmojiContent = transform.Find("Content/Body/ListEmoji/Viewport/Content").GetComponent<RectTransform>();
		//
		m_prefCategoryItem = transform.Find("Content/Body/Category/Viewport/Content/BtnTabItem").gameObject;
		m_prefCategoryItem.SetActive(false);
		m_prefCategoryContainerItem = transform.Find("Content/Body/ListEmoji/Viewport/CategoryContainer").gameObject;
		m_prefCategoryContainerItem.SetActive(false);
		m_prefEmojiItem = transform.Find("Content/Body/ListEmoji/Viewport/BtnEmojiItem").gameObject;
		m_prefEmojiItem.SetActive(false);

		CreateEmojiObjects(data);

		//add listener
		m_btnClose.onClick.AddListener(CloseOnClick);
		m_btnDone.onClick.AddListener(DoneOnClick);
		m_btnClearOutput.onClick.AddListener(ClearOutputOnClick);

		//register event
		HAGOEmojiPickerControl.Api.OnCategorySelectedEvent += OnCategorySelectedHandler;
		HAGOEmojiPickerControl.Api.OnVisibleCategoryItemEvent += OnVisibleCategoryItemHandler;
		HAGOEmojiPickerControl.Api.OnGetLowestIndexVisibleCategoryEvent += OnGetLowestIndexVisibleCategoryHandler;

		//init child element
		m_scrListEMojiOcclusion.Init();

		m_isCacheLoadData = isCacheLoad;
		//ShowPopup();
	}

    public void HideView()
    {
        m_canvas.alpha = 0;
        m_canvas.interactable = false;
    }

	private void CreateEmojiObjects(Dictionary<string, List<string>> data)
    {
		StartCoroutine(IGenerateEmojiObjects(data));
	}

	IEnumerator IGenerateEmojiObjects(Dictionary<string, List<string>> data)
	{
		m_firstCategoryItemView = null;
		foreach (KeyValuePair<string, List<string>> category in data)
		{
			HAGOCategoryItemView categoryItemView = CreateCategoryItem(category.Key);
            if (categoryItemView)
            {
				m_listCategoryItemView.Add(categoryItemView);
			}
			Transform categoryContainer = Instantiate(m_prefCategoryContainerItem, m_rectEmojiContent).transform;
			categoryContainer.gameObject.SetActive(true);
			HAGOContainerEmojiItemView categoryContainerView = categoryContainer.GetComponent<HAGOContainerEmojiItemView>();
            if (categoryContainerView)
            {
				categoryContainerView.Init(m_scrListEmoji);
			}

			if (m_firstCategoryItemView == null)
			{
				m_firstCategoryItemView = categoryItemView;
			}

            StartCoroutine(ICreateObjInCategory(category.Value, categoryContainer));
       
            if (!m_dictContainerEmoji.ContainsKey(category.Key))
			{
				m_dictContainerEmoji.Add(category.Key, categoryContainer as RectTransform);
			}
			yield return m_delayCreateCategory;

		}

        //force show first category item
        m_firstCategoryItemView.ItemOnClick();

		Debug.Log("Complete Load all emoji data");
		//Load complete data
		HAGOEmojiPickerControl.Api.CompleteLoadData();
		m_canvas.alpha = 1;
		m_canvas.interactable = true;
		//
	}

	private IEnumerator ICreateObjInCategory(List<string> listEmoji, Transform categoryContainer)
    {
		byte countObj = 0;
        if (listEmoji!= null)
        {
            for (int i = 0; i < listEmoji.Count; i++)
            {
				CreateItem(listEmoji[i], categoryContainer);
				countObj++;
				if (countObj >= 100)
				{
					countObj = 0;
					yield return null;
				}
			}
        }
		
	}

	private void DoneOnClick()
    {
		if (!m_isCacheLoadData)
		{
			m_cvgButtons.interactable = false;
		}
		ClosePopup(() => HAGOEmojiPickerControl.Api.CompleteEmojiPicker(m_result,m_isCacheLoadData));
    }

    private void CloseOnClick()
    {
        if (!m_isCacheLoadData)
        {
			m_cvgButtons.interactable = false;
		}
		ClosePopup(() => HAGOEmojiPickerControl.Api.Exit(m_isCacheLoadData));
    }

    private void ClearOutputOnClick()
    {
        OnClearOutputHandler();
    }
	
    private void OnCategorySelectedHandler(string category)
    {
		m_isScrollingSeletectedCategory = true;
		HAGOTweenUtils.ScrollTo(m_scrListEmoji, m_dictContainerEmoji[category],()=> { m_isScrollingSeletectedCategory = false; });
	}

	private void OnGetLowestIndexVisibleCategoryHandler(int index)
	{
        if (m_isScrollingSeletectedCategory)
        {
			return;
        }
		Vector2 endTarget = (Vector2)m_scrTitleCategory.transform.InverseTransformPoint(m_scrTitleCategory.content.position)
			- (Vector2)m_scrTitleCategory.transform.InverseTransformPoint(m_listCategoryItemView[index].transform.position);
		m_scrTitleCategory.content.anchoredPosition = Vector2.Lerp(m_scrTitleCategory.content.anchoredPosition, endTarget, 0.1f);
		if (m_scrTitleCategory.content.anchoredPosition.x <= m_rectScrCategory.sizeDelta.x - m_scrTitleCategory.content.sizeDelta.x)
		{
			endTarget.x = m_rectScrCategory.sizeDelta.x - m_scrTitleCategory.content.sizeDelta.x;
			endTarget.y = m_scrTitleCategory.content.anchoredPosition.y;
			m_scrTitleCategory.content.anchoredPosition = endTarget;
		}
	}

	private void OnVisibleCategoryItemHandler(int indexCategory, bool isActive)
	{
		m_listCategoryItemView[indexCategory].HightLight(isActive);
	}

	private HAGOCategoryItemView CreateCategoryItem(string category)
	{
		GameObject go = Instantiate(m_prefCategoryItem, m_rectCategoryContent);
		go.SetActive(true);
		HAGOCategoryItemView itemView = go.GetComponent<HAGOCategoryItemView>();
		itemView.Init(category);

		return itemView;
	}

	private void CreateItem(string emoji, Transform content)
    {
		m_cacheEmojiObj = Instantiate(m_prefEmojiItem, content);
		m_cacheEmojiObj.SetActive(true);
		m_cacheEmojiObj.GetComponent<HAGOEmojiItemView>().Init(emoji, OnEmojiSelectedHandler);
    }
	
	private void OnClearOutputHandler()
	{
		m_result = string.Empty;

		UpdateOutputText(m_result);
	}

	private void OnEmojiSelectedHandler(string data)
	{
		m_result += data;

		UpdateOutputText(m_result);
	}

	private void UpdateOutputText(string data)
	{
		m_tmpOuput.text = data;
	}

	public void ShowPopup()
	{
		HAGOTweenUtils.ShowPopup(m_canvas, m_content);
	}

	public void HidePopup(Action callback = null)
    {
		HAGOTweenUtils.HidePopup(m_canvas, m_content, callback,false);
	}

	private void ClosePopup(Action callback)
	{
		HAGOTweenUtils.HidePopup(
			m_canvas,
			m_content,
			callback,
			false
		);
	}

	public void ActiveCache(bool isNextCacheData)
    {
		m_isCacheLoadData = isNextCacheData;
		m_firstCategoryItemView.ItemOnClick();
		OnClearOutputHandler();
		ShowPopup();
		m_rectCategoryContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
	}
}
