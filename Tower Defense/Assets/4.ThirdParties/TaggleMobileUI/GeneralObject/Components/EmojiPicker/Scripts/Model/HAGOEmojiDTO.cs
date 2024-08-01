using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOEmojiDTO
{
    public long Name { get; set; }
    public string Unicode { get; set; }

	public HAGOEmojiDTO(long name, string unicode)
	{
		Name = name;
		Unicode = unicode;
	}
}