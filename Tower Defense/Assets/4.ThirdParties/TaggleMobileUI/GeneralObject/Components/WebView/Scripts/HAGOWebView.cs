using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HAGOWebView : MonoBehaviour
{
    private GameObject m_loader;
    private Button m_btnExit;
    private Button m_btnNext;
    private Button m_btnBack;
    private RectTransform m_webviewContent;

    //param
    private UniWebView m_webview;

    public enum ViewType
    {
        Web,
        DocUrl,
        Youtube
    }

    //const
    public const string CONFIG_GOOGLE_VIEWER = "https://docs.google.com/viewer?embedded=true&url={0}";
    public const string CONFIG_GOOGLE_DOC_ROOT = "https://docs.google.com";

    public static void Init(string url, Action onClose, ViewType viewType = ViewType.Web)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>(HAGOConstant.PREFAB_WEBVIEW));
        go.GetComponent<HAGOWebView>().InitView(viewType, url, onClose);
    }

    public void InitView(ViewType viewType, string url, Action onClose)
    {
        //find reference
        m_btnExit = transform.Find("Canvas/Content/ToolBar/BtnDone").GetComponent<Button>();
        m_btnNext = transform.Find("Canvas/Content/ToolBar/Navigation/BtnForward").GetComponent<Button>();
        m_btnBack = transform.Find("Canvas/Content/ToolBar/Navigation/BtnBack").GetComponent<Button>();
        m_loader = transform.Find("Canvas/Content/ToolBar/Loader").gameObject;
        m_webviewContent = transform.Find("Canvas/Content/Container") as RectTransform;
        m_webview = transform.Find("Canvas/Content/Container").gameObject.AddComponent<UniWebView>();


        StartCoroutine(IEInitWebview(viewType, url, onClose));

        //add listener
        m_btnExit.onClick.AddListener(ExitOnClick);
        m_btnNext.onClick.AddListener(NextOnClick);
        m_btnBack.onClick.AddListener(BackOnClick);
    }

    private void BackOnClick()
    {
        if(m_webview.CanGoBack)
            m_webview.GoBack();
    }

    private void NextOnClick()
    {
        if(m_webview.CanGoForward)
            m_webview.GoForward();
    }

    private void ExitOnClick()
    {
        m_webview.InternalOnShouldClose();
    }

    private IEnumerator IEInitWebview(ViewType viewType, string url, Action onClose)
    {
        //Delay 5 frames to wait UI effect position
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        
        UniWebView.ClearCookies();
        Debug.Log($"////// Init web view [{viewType.ToString()}] data.Url: {url}");
        
        #if UNITY_ANDROID
        // check if Youtube set Landscape
        if(viewType == ViewType.Youtube)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        #endif

        // Full screen
        m_webview.Frame = new Rect(0, 0, Screen.width, Screen.height);
        // Full screen with landscape mode
        m_webview.OnOrientationChanged += (view, orientation) => {
            m_webview.Frame = new Rect(0, 0, Screen.width, Screen.height);
        };
        UniWebView.SetWebContentsDebuggingEnabled(true);
        // m_webview.AddPermissionTrustDomain(APIpaths.RESTRoot.Replace("https://", "")); //TODO: handle later when dll available
        
        // allow local file access
        m_webview.SetAllowFileAccessFromFileURLs(true);
        m_webview.ReferenceRectTransform = m_webviewContent;
        m_webview.SetShowToolbar(false, false, false, false);
        // allow zoom
        m_webview.SetZoomEnabled(true);
        // auto fit screen : only work with Android
        m_webview.SetLoadWithOverviewMode(true);
  
        // disable scroll checking for now
        m_webview.OnPageStarted += OnPageStarted;
        m_webview.OnPageFinished += OnPageFinished;
        m_webview.OnPageErrorReceived += OnPageErrorReceived;
        m_webview.OnShouldClose += (view) =>
        {
            // get progress
            m_webview.OnPageStarted -= OnPageStarted;
            m_webview.OnPageFinished -= OnPageFinished;
            m_webview.OnPageErrorReceived -= OnPageErrorReceived;
            onClose?.Invoke();
            OnCloseWebViewHandler();
            return true;
        };
        //
        #if UNITY_ANDROID
                // handle pdf viewer for Android. Default behaviour is download
                if (viewType == ViewType.DocUrl)
                {
                    string fileExt = GetFileExtensionFromUrl(url);
                    if (fileExt.ToLower() == ".pdf")
                    {
                        m_webview.AddPermissionTrustDomain(CONFIG_GOOGLE_DOC_ROOT.Replace("https://", ""));
                        url = string.Format(CONFIG_GOOGLE_VIEWER, Uri.EscapeDataString(url));
                        Debug.Log("Open url: " + url);
                    }
                }
        #endif
        //
        m_webview.Load(url);
        m_webview.Show(true, UniWebViewTransitionEdge.Bottom, 0.35f);
    }

    public string GetFileExtensionFromUrl(string url)
    {
        url = url.Split('?')[0];
        url = url.Split('/').Last();
        return url.Contains('.') ? url.Substring(url.LastIndexOf('.')) : "";
    }

    //note: for force close webview, using CloseWebView() instead Close()
    private void OnCloseWebViewHandler()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        m_webview.Stop();
        m_webview.CleanCache();
        UniWebView.ClearCookies();
        Destroy(gameObject);
    }

    private void OnPageStarted(UniWebView webView, string url)
    {
        ShowLoader(true);
    }

    private void OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        m_webview = webView;

        ShowLoader(false);
        //
        m_btnBack.interactable = webView.CanGoBack;
        m_btnNext.interactable = webView.CanGoForward;

        Debug.Log("On page load finished: " + statusCode);

        if (url.StartsWith(CONFIG_GOOGLE_DOC_ROOT))
        {
            webView.EvaluateJavaScript("document.title", (payload) =>
            {
                if (payload.resultCode.Equals("0"))
                {
                    string title = payload.data;
                    Debug.Log(string.Format("Get docutment title: {0}", title));
                    //// fix from https://stackoverflow.com/questions/53985055/pdf-sometimes-not-loading-with-google-embedded-viewer-on-android
                    if (title.Trim().ToString() == "")
                    {
                        StartCoroutine(ReloadAfter(0.5f, url));
                    }
                }
            });
        }
    }

    private void OnPageErrorReceived(UniWebView webView, int statusCode, string url)
    {
        Debug.Log("On page load error: " + statusCode);
    }

    private void ShowLoader(bool isLoading)
    {
        m_loader.SetActive(isLoading);
    }

    private IEnumerator ReloadAfter(float seconds, string url)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("Reloading web page after 0.5s");
        m_webview.Load(url);
    }
}
