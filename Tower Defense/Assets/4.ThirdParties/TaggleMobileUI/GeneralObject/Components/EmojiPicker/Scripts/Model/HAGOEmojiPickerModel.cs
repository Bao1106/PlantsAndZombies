using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAGOEmojiPickerModel
{
    private static HAGOEmojiPickerModel m_api;
    public static HAGOEmojiPickerModel Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGOEmojiPickerModel();
            }
            return m_api;
        }
    }

    //config
    public Dictionary<string, List<string>> EmojiData;

    public bool IsEmojiDataLoaded()
    {
        return HAGOEmojiPickerModel.Api.EmojiData != null;
    }
}