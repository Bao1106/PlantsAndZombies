using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BiometricAuthenticationManager : MonoBehaviour
{

    public static Action<string> AuthenticationBioEvent;
    public static Action<string> AuthenticationBioPublicKeyEvent;
    public static Action<string> AuthenticationBioSignatureCodeEvent;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Start()
    {
        
    }

    public void AuthenticationBioCallback(string result)
    {
        AuthenticationBioEvent?.Invoke(result);
    }

    public void AuthenticationBioPublicKeyCallback(string result)
    {
        AuthenticationBioPublicKeyEvent?.Invoke(result);
    }
    public void AuthenticationBioSignatureCodeCallback(string result)
    {
        AuthenticationBioSignatureCodeEvent?.Invoke(result);
    }
}
