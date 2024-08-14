using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUIToggleOptionDTO
{
    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_ID)]
    public string ID { get; set; }

    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_TITLE)]
    public string Title { get; set; }

    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_KEY_FORM)]
    public string KeyForm { get; set; }
    
    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_DEFAULT_VALUE)]
    public bool DefaulValue { get; set; }
    
    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_IS_REQUIRED)]
    public bool IsRequired { get; set; }

	public HAGOUIToggleOptionDTO(bool isOn, bool isRequired = false)
	{
		DefaulValue = isOn;
		IsRequired = isRequired;
	}
 
	public HAGOUIToggleOptionDTO(string id, string title, string keyForm, bool defaultValue, bool isRequired = false)
	{
		ID = id;
		Title = title;
		KeyForm = keyForm;
        DefaulValue = defaultValue;
		IsRequired = isRequired;
	}

    public HAGOUIToggleOptionDTO(JObject data)
	{
		ID = data.Value<string>(HAGOServiceKey.PARAM_ID);
		string title = data.Value<string>(HAGOServiceKey.PARAM_TITLE);
		Title = HAGOUtils.IsLangKey(title) ? I18N.instance.getValue(title) : title;
		KeyForm = data.Value<string>(HAGOServiceKey.PARAM_KEY_FORM);
        DefaulValue = data.Value<bool>(HAGOServiceKey.PARAM_DEFAULT_VALUE);
		IsRequired = false;
	}
}