using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HAGOSkinHelper : MonoBehaviour
{
	public HAGOSkinSprites[] Skins;

	private Image m_Image;
	private Text m_Text;

	//param
	private bool m_IsInitComplete = false;
	private Sprite m_DefaultImage;
	private Color m_DefaultColor;

	private void OnEnable()
	{
		if (!m_IsInitComplete)
		{
			Init();
		}

		UpdateSkin(HAGOModel.Api.GetCacheAppSkin());
	}

	private void OnDestroy()
	{
		if (m_IsInitComplete)
		{
			HAGOControl.Api.OnSkinChangedEvent -= OnSkinChangedHandler;
		}
	}

	private void Init()
	{
		m_Text = GetComponent<Text>();
		m_Image = GetComponent<Image>();
		m_DefaultImage = m_Image?.sprite ?? null;
		m_DefaultColor = m_Image?.color ?? (m_Text?.color ?? Color.white);

		m_IsInitComplete = true;

		HAGOControl.Api.OnSkinChangedEvent += OnSkinChangedHandler;
	}

	private void OnSkinChangedHandler(HAGOSkinType skin)
	{
		UpdateSkin(skin);
	}

	private void UpdateSkin(HAGOSkinType skin)
	{
		//images skin set (not image component)
		if (Skins == null || Skins.Length == 0)
			return;
		
		Sprite newSprite = m_DefaultImage;
		Color newColor = m_DefaultColor;

		bool ignoreColor = false;
		//
		for (int i = 0; i < Skins.Length; i++)
		{
			if (Skins[i].SkinType == skin)
			{
				ignoreColor = Skins[i].IgnoreColor;
				newSprite = Skins[i].Image;
				newColor = Skins[i].Color;
				break;
			}
		}

		//override if default skin primary color
		bool isOverridePrimaryColor = false;
		//
		if(skin == HAGOSkinType.Default && !string.IsNullOrEmpty(HAGOModel.Api.PrimaryColor))
		{
			isOverridePrimaryColor = newColor == HAGOUtils.ParseColorFromString(HAGOConstant.COLOR_PRIMARY);
			
			if(isOverridePrimaryColor)
			{
				newColor = newColor == HAGOUtils.ParseColorFromString(HAGOConstant.COLOR_PRIMARY) ? HAGOUtils.ParseColorFromString(HAGOModel.Api.PrimaryColor) : newColor;
			}
		}
		
		//update view
		if(!ignoreColor || isOverridePrimaryColor)
		{
			UpdateColor(newColor);
		}

		//update component
		if(m_Image != null)
		{
			m_Image.sprite = newSprite;
		}
	}

	private void UpdateColor(Color newColor)
	{
		if(m_Image != null)
		{
			m_Image.color = newColor;
		}
		else if(m_Text != null)
		{
			m_Text.color = newColor;
		}
	}
}

/// <summary>
/// Helper class, containing sprite parameters.
/// </summary>
[Serializable]
public class HAGOSkinSprites
{
	#region PUBLIC VARS

	/// <summary>
	/// Skin type.
	/// </summary>
	public HAGOSkinType SkinType;

	/// <summary>
	/// Sprite.
	/// </summary>
	public Sprite Image;

	/// <summary>
	/// Color sprite.
	/// </summary>
	public Color Color;

	/// <summary>
	/// Color change ignore.
	/// </summary>
	public bool IgnoreColor = false;

	#endregion
}
