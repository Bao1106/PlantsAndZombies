﻿using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUIDateTimeComponentDTO
{
	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_ID)]
	public string ID { get; set; }

	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_TITLE)]
	public string Title { get; set; }

	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_KEY_FORM)]
	public string KeyForm { get; set; }
	
	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_VALUE)]
	public long ValueLong { get; set; }

	[JsonIgnore]
	public DateTime Value { get { return IsDurationPicker ? DateTime.MinValue.AddSeconds(ValueLong) : HAGOUtils.GetDateTimeFromEpoch(ValueLong); } }

	[JsonIgnore]
	public bool IsDurationPicker { get; set; }

	public HAGOUIDateTimeComponentDTO(string id, string title, string keyForm, DateTime value, bool isDurationPicker = false)
	{
		ID = id; 
		Title = title;
		KeyForm = keyForm;
		ValueLong = isDurationPicker ? value.Second : HAGOUtils.GetEpochTimeFromDateTime(value);
		IsDurationPicker = isDurationPicker;
	}

	public HAGOUIDateTimeComponentDTO(JObject data)
	{
		ID = data.Value<string>(HAGOServiceKey.PARAM_ID);
		string title = data.Value<string>(HAGOServiceKey.PARAM_TITLE);
		Title = HAGOUtils.IsLangKey(title) ? I18N.instance.getValue(title) : title;
		KeyForm = data.Value<string>(HAGOServiceKey.PARAM_KEY_FORM);
		
		long value = data.Value<long?>(HAGOServiceKey.PARAM_VALUE) ?? -1;
		if(value != -1)
		{
			ValueLong = data.Value<long>(HAGOServiceKey.PARAM_VALUE);
		}
		else
		{
			ValueLong = HAGOUtils.GetEpochTimeFromDateTime(DateTime.Now);
		}
	}
}
