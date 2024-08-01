using System.Collections;
using System.Collections.Generic;
using Honeti;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUIToggleListDTO
{
	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_ID)]
	public string ID { get; set; }

	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_TITLE)]
	public string Title { get; set; }

	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_KEY_FORM)]
	public string KeyForm { get; set; }
	
	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_OPTIONS)]
	public List<HAGOUIToggleOptionDTO> Options { get; set; }

    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_IS_REQUIRED)]
	public bool IsRequired { get; set; }

	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_IS_SELECT_MULTIPLE)]
	public bool IsSelectMultiple { get; set; }

	[JsonIgnore]
	public string FormType { get; set; } //format style: checkbox, toggle, slider,...

	[JsonIgnore]
	public JObject Advanced { get; set; }

	public HAGOUIToggleListDTO(string id, string title, string keyForm, List<HAGOUIToggleOptionDTO> options, string formType = HAGOServiceKey.PARAM_JSON_FORM_RADIOS, JObject advanced = null, bool isSelectMultiple = false, bool isRequired = false)
	{
		ID = id; 
		Title = title;
		KeyForm = keyForm;
		Options = options;
		FormType = formType;
		Advanced = advanced;
		IsSelectMultiple = isSelectMultiple;
		IsRequired = isRequired;
	}

	public HAGOUIToggleListDTO(JObject data)
	{
		ID = data.Value<string>(HAGOServiceKey.PARAM_ID); 
		string title = data.Value<string>(HAGOServiceKey.PARAM_TITLE);
		Title = HAGOUtils.IsLangKey(title) ? I18N.instance.getValue(title) : title;
		KeyForm = data.Value<string>(HAGOServiceKey.PARAM_KEY_FORM);
		//
		Options = new List<HAGOUIToggleOptionDTO>();
		JArray ja = data.Value<JArray>(HAGOServiceKey.PARAM_OPTIONS);
		foreach(JToken jt in ja)
		{
			HAGOUIToggleOptionDTO optDTO = new HAGOUIToggleOptionDTO((JObject)jt);
			Options.Add(optDTO);
		}
	}
}