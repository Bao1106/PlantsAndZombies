using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUIJsonFormDataDTO
{
	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_FORM_ID)]
	public long FormId;

	[JsonProperty(PropertyName = HAGOServiceKey.PARAM_DATA)]
	public Dictionary<string, JToken> Data; //<keyForm, jsonValue>

	public HAGOUIJsonFormDataDTO(long formId, Dictionary<string, JToken> data)
	{
		FormId = formId;
		Data = data;
	}

	public HAGOUIJsonFormDataDTO(JObject data)
	{
		FormId = data.Value<long>(HAGOServiceKey.PARAM_FORM_ID);
		Data = data.Value<Dictionary<string, JToken>>(HAGOServiceKey.PARAM_DATA);
	}

	public string GetRawData()
	{
		if(Data == null)
		{
			return string.Empty;
		}
		else
		{
			return JsonConvert.SerializeObject(GetJObjectData());
		}
	}

	public JObject GetJObjectData()
	{
		if(Data == null)
		{
			return null;
		}
		else
		{
			return HAGOJSONFlattener.Unflatten(Data);
		}
	}
}

public class HAGOUIJsonFormDataResultDTO
{
	public bool IsSuccess { get; set; }
	public HAGOUIJsonFormDataDTO Data { get; set; }

	public HAGOUIJsonFormDataResultDTO(bool isSuccess, HAGOUIJsonFormDataDTO data)
	{
		IsSuccess = isSuccess;
		Data = data;
	}
}