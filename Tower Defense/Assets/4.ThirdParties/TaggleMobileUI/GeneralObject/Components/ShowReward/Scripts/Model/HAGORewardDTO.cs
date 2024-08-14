using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGORewardShortDTO
{
    public long Id { get; set; }
	public string Desc { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public int Quantity { get; set; }

	public HAGORewardShortDTO(long id, string name, string icon, int quantity, string desc = "")
	{
		Id = id;
		Name = name;
		Image = icon;
    	Quantity = quantity;
		Desc = desc;
	}
}