using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOItemPickerDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Icon { get; set; }
	public bool IsSelected { get; set; }
	public JObject ExtraData { get; set; }

	public HAGOItemPickerDTO(string id, string name, string desc, string icon, bool isSelected = false,JObject extraData = null)
	{
		Id = id;
		Name = name;
		Desc = desc;
		Icon = icon;
		IsSelected = isSelected;
    	ExtraData = extraData;
	}
}