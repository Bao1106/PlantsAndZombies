using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using Newtonsoft.Json;
using UnityEngine;

public class HAGOBioAuthManager : MonoBehaviour
{
	private static HAGOBioAuthManager m_api;
    public static HAGOBioAuthManager Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new GameObject("HAGOBioAuthManager").AddComponent<HAGOBioAuthManager>();
            }
            return m_api;
        }
    }

    public Action<BiometricData> ResultCallbackEvent;
    private bool m_isCallbackAlready = false;

	public void Init(Action<BiometricData> callback)
	{
        ResultCallbackEvent = callback;

        if(Application.isEditor)
        {
            //auto response true when editor
            ResultCallbackEvent?.Invoke(new BiometricData());
            Destroy(this.gameObject);
        }
        else
        {
            BiometricAuthentication.StartBioAuthenticationCustom(this.gameObject.name);
        }
	}

    //fixed name, if rename will not receive callback for using bio auth custom
    public void AuthenticationBioCustomCallback(string result)
    {
        if(m_isCallbackAlready)
        {
            return;
        }

        m_isCallbackAlready = true;

        Debug.Log(" Authentication Bio Callback is called with message: " + result);

        BiometricData data = JsonConvert.DeserializeObject<BiometricData>(result);
        ResultCallbackEvent?.Invoke(data);
        
        Destroy(this.gameObject);
    }
}