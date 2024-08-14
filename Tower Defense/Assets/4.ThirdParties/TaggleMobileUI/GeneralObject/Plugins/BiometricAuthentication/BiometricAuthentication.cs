using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class BiometricAuthentication : MonoBehaviour
{
	private static BiometricAuthentication _instance;

	public static BiometricAuthentication Instance
	{
		get
		{
			if (_instance == null)
			{
				var obj = new GameObject("BiometricAuthentication");
				_instance = obj.AddComponent<BiometricAuthentication>();
			}

			return _instance;
		}
	}
	private void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
	}

	public static void StartBioAuthentication()
	{
#if UNITY_EDITOR
		// Do nothing
#elif UNITY_IOS
		BiometricAuthenticationIOS.StartBioAuthentication();
#else
		BiometricAuthenticationAndroid.StartBioAuthentication();
#endif
	}
	public static void StartBioAuthenticationCustom(string unityObject)
	{
#if UNITY_EDITOR
		// Do nothing
#elif UNITY_IOS
		BiometricAuthenticationIOS.StartBioAuthenticationCustom(unityObject);
#else
		BiometricAuthenticationAndroid.StartBioAuthenticationCustom(unityObject);
#endif
	}
	public static void StartBioPublicKey()
	{
#if UNITY_EDITOR
		// Do nothing
#elif UNITY_IOS
		BiometricAuthenticationIOS.StartBioPublicKey();
#else
		BiometricAuthenticationAndroid.StartBioPublicKey();
#endif
	}
	public static void StartBiometricSignatureCode(string challenge)
	{
#if UNITY_EDITOR
		// Do nothing
#elif UNITY_IOS
		BiometricAuthenticationIOS.StartBiometricSignatureCode(challenge);
#else
		BiometricAuthenticationAndroid.StartBiometricSignatureCode(challenge);
#endif
	}
	public static void DeleteBioPublicKey()
	{
#if UNITY_EDITOR
		// Do nothing
#elif UNITY_IOS
		BiometricAuthenticationIOS.DeleteBioPublicKey();
#else
		BiometricAuthenticationAndroid.DeleteBioPublicKey();
#endif
	}
}

public class BiometricAuthenticationIOS
{
#if UNITY_IOS //&& !UNITY_EDITOR

	[DllImport("__Internal")]
	private static extern void _startBiometricAuthentication();
	[DllImport("__Internal")]
	private static extern void _startBiometricAuthenticationCustom(string UnityObjectName);
	[DllImport("__Internal")]
	private static extern void _startBiometricPublicKey();
	[DllImport("__Internal")]
	private static extern void _deleteBiometricPublicKey();
	[DllImport("__Internal")]
	private static extern void _startBiometricSignatureCode(string signMessage);

	public static void StartBioAuthentication()
	{
		_startBiometricAuthentication();
	}

	public static void StartBioAuthenticationCustom(string unityObject)
	{
		_startBiometricAuthenticationCustom(unityObject);

	}
	public static void StartBioPublicKey()
	{
		_startBiometricPublicKey();

	}
	public static void DeleteBioPublicKey()
	{
		_deleteBiometricPublicKey();

	}
	public static void StartBiometricSignatureCode(string signMessage)
	{
		_startBiometricSignatureCode(signMessage);
	}

#endif
}

public class BiometricAuthenticationAndroid
{
	public static void StartBioAuthentication()
	{
		using (AndroidJavaClass utils = new AndroidJavaClass("com.taggle.biometricauthentication.BiometricBridge"))
		{
			utils.CallStatic("StartBiometricAuthentication");
		}

	}
	public static void StartBioAuthenticationCustom(string unityObject)
	{
		using (AndroidJavaClass utils = new AndroidJavaClass("com.taggle.biometricauthentication.BiometricBridge"))
		{
			utils.CallStatic("StartBiometricAuthenticationCustom", unityObject);
		}

	}
	public static void StartBioPublicKey()
	{
		using (AndroidJavaClass utils = new AndroidJavaClass("com.taggle.biometricauthentication.BiometricBridge"))
		{
			utils.CallStatic("StartBiometricPublicKey");
		}

	}
	public static void DeleteBioPublicKey()
	{
		using (AndroidJavaClass utils = new AndroidJavaClass("com.taggle.biometricauthentication.BiometricBridge"))
		{
			utils.CallStatic("DeleteBiometricPublicKey");
		}

	}
	public static void StartBiometricSignatureCode(string challenge)
	{
		using (AndroidJavaClass utils = new AndroidJavaClass("com.taggle.biometricauthentication.BiometricBridge"))
		{
			utils.CallStatic("StartBiometricSignatureCode", challenge);
		}

	}
}