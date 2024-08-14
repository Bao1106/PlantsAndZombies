using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUITextDTO
{
    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_ID)]
    public string ID { get; set; }

    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_TITLE)]
    public string Title { get; set; }

    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_CONTENT)]
    public string Content { get; set; }

    [JsonProperty(PropertyName = HAGOServiceKey.PARAM_KEY_FORM)]
	public string KeyForm { get; set; }
    
	public HAGOUITextDTO(string id, string title, string content, string keyForm)
	{
		ID = id;
		Title = title;
        Content = content;
		KeyForm = keyForm;
	}
}