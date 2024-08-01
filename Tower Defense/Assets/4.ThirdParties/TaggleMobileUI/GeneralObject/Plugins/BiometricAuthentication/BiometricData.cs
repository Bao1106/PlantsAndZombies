using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class BiometricData 
{
    [JsonProperty("result_code")]
    private string m_ResultCode;

    [JsonProperty("callback_name")]
    public string CallbackName;

    [JsonProperty("value")]
    public string Value;

    [JsonIgnore]
    public bool IsSuccess => m_ResultCode.Equals("1");

    public BiometricData()
    {
        #if UNITY_EDITOR
        m_ResultCode = "1";
        #endif
    }
}
