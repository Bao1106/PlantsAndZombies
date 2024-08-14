using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HAGOSkinType
{
    Default,
    Kids
}

public class HAGOModel
{
    private static HAGOModel m_api;
    public static HAGOModel Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGOModel();
            }
            return m_api;
        }
    }

    // Params
    public bool IsLoading { get; set; }
    public bool IsInitSkin { get; set; } = false;

    // Cache data
    public string PrimaryColor { get; set; } = HAGOConstant.COLOR_PRIMARY;
    //
    private HAGOSkinType m_SkinType { get; set; } = HAGOSkinType.Default;

    public void CacheAppSkin(HAGOSkinType skinType)
    {
        m_SkinType = skinType;
        //
        HAGOModel.Api.IsInitSkin = true;
    }

    public HAGOSkinType GetCacheAppSkin()
    {
        return m_SkinType;
    }
}