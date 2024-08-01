using System;
using System.Collections;
using System.Collections.Generic;
using Kyub.EmojiSearch.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOEmojiPickerManager : MonoBehaviour
{
	private static HAGOEmojiPickerManager m_api;
    public static HAGOEmojiPickerManager Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = Instantiate(Resources.Load<GameObject>(HAGOConstant.PREFAB_EMOJI_PICKER)).GetComponent<HAGOEmojiPickerManager>();
            }
            return m_api;
        }
    }

    private HAGOEmojiPickerView m_view;

	public void Init(Action<string> onCloseEvent = null, bool isCacheLoadData = true)
	{
        if(!HAGOModel.Api.IsLoading)
        {
            StartCoroutine(IEInit(onCloseEvent, isCacheLoadData));
        }
        else 
        {
            if (!gameObject.activeInHierarchy)
            {
                SetActive(true);
            }
            ActiveCache(onCloseEvent, isCacheLoadData);
        }
    }

    public void LoadToCacheData(Action onCompleteLoadData = null)
    {
        if (!HAGOModel.Api.IsLoading)
        {
            StartCoroutine(ILoadToCache(onCompleteLoadData));
        }
        else
        {
            onCompleteLoadData?.Invoke();
        }
    }    

    private IEnumerator ILoadToCache(Action onCompleteLoadData = null)
    {
        HAGOModel.Api.IsLoading = true;

        HAGOEmojiPickerControl.Api.CallBackCompleteLoadDataEvent = ()=> {
            SetActive(false);
            onCompleteLoadData?.Invoke();
        };

        //init view
        m_view = transform.Find("Canvas").GetComponent<HAGOEmojiPickerView>();

        if (!HAGOEmojiPickerModel.Api.IsEmojiDataLoaded())
        {
            TextAsset file = Resources.Load<TextAsset>("EmojiDataAssets/Sheets/EmojiData");
            if (file != null)
            {
                Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
                JArray jaData = JsonConvert.DeserializeObject<JArray>(file.text);

                foreach (JObject joEmoji in jaData)
                {
                    string category = joEmoji.Value<string>("category");
                    string emoji = joEmoji.Value<string>("unified").PadLeft(8, '0');

                    if (!emoji.Contains("-"))
                    {
                        //format string
                        emoji = HAGOUtils.ReplaceEmojiName(emoji);

                        if (!data.ContainsKey(category))
                        {
                            data.Add(category, new List<string>() { emoji });
                        }
                        else
                        {
                            data[category].Add(emoji);
                        }
                    }
                }

                //cache for next time usage
                HAGOEmojiPickerModel.Api.EmojiData = data;
            }
        }
        m_view.Init(HAGOEmojiPickerModel.Api.EmojiData, true);
        m_view.HideView();
        yield return null;
    }

    private IEnumerator IEInit(Action<string> onCloseEvent = null, bool isCacheLoadData = false)
    {
        HAGOModel.Api.IsLoading = true;

        HAGOEmojiPickerControl.Api.ResultCallbackEvent = onCloseEvent;

        //init view
        m_view = transform.Find("Canvas").GetComponent<HAGOEmojiPickerView>();
        
        if(!HAGOEmojiPickerModel.Api.IsEmojiDataLoaded())
        {
            TextAsset file = Resources.Load<TextAsset>("EmojiDataAssets/Sheets/EmojiData");
            if(file != null)
            {
                Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
                JArray jaData = JsonConvert.DeserializeObject<JArray>(file.text);

                foreach (JObject joEmoji in jaData)
                {
                    string category = joEmoji.Value<string>("category");
                    string emoji = joEmoji.Value<string>("unified").PadLeft(8, '0');

                    if (!emoji.Contains("-"))
                    {
                        //format string
                        emoji = HAGOUtils.ReplaceEmojiName(emoji);

                        if (!data.ContainsKey(category))
                        {
                            data.Add(category, new List<string>() { emoji });
                        }
                        else
                        {
                            data[category].Add(emoji);
                        }
                    }
                }

                //cache for next time usage
                HAGOEmojiPickerModel.Api.EmojiData = data;
            }
        }

        m_view.Init(HAGOEmojiPickerModel.Api.EmojiData, isCacheLoadData);
        m_view.ShowPopup();

        if (!isCacheLoadData)
        {
            HAGOModel.Api.IsLoading = false;
        }
        yield return null;

    }

    private void ActiveCache(Action<string> onCloseEvent = null, bool isCacheLoadData = false)
    {
        try
        {
            HAGOModel.Api.IsLoading = true;
            HAGOEmojiPickerControl.Api.ResultCallbackEvent = onCloseEvent;

            if(m_view == null)
            {
                //init view
                m_view = transform.Find("Canvas").GetComponent<HAGOEmojiPickerView>();
            }
            //
            m_view.ActiveCache(isCacheLoadData);

            if (!isCacheLoadData)
            {
                HAGOModel.Api.IsLoading = false;
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Emoji Exception: " + ex.ToString());
        }
    }

    public void Exit(bool isCacheLoad = false)
    {
        if (!isCacheLoad)
        {
            m_view.Destroy();
            Destroy(this.gameObject);
            HAGOModel.Api.IsLoading = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}