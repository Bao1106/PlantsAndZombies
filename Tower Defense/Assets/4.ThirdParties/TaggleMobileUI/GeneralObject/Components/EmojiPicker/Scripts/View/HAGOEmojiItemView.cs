using System;
using System.Collections;
using System.Collections.Generic;
using Kyub.EmojiSearch.UI;
using UnityEngine;
using UnityEngine.UI;

public class HAGOEmojiItemView : MonoBehaviour
{
    private Button m_btnItem;
    private TMP_EmojiTextUGUI m_tmpEmoji;

    //param
    public string m_data;
    public Action<string> m_onClickEvent;

    public void Init(string data, Action<string> onClickEvent)
    {
        m_data = data;
        m_onClickEvent = onClickEvent;

        //find reference
        m_btnItem = GetComponent<Button>();
		m_tmpEmoji = transform.Find("TMPEmoji").GetComponent<TMP_EmojiTextUGUI>();

        //update view
        m_tmpEmoji.text = data;

        //add listener
        m_btnItem.onClick.AddListener(ItemOnClick);

    }
    private void ItemOnClick()
    {
        m_onClickEvent?.Invoke(m_data);
    }

}
