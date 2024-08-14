using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResourceLoader
{
    private static ResourceLoader m_api;

    public static ResourceLoader Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new ResourceLoader();
            }
            return m_api;
        }
    }

    private ResourceLoader()
    {
        m_cache = new Dictionary<string, object>();
    }

    private Dictionary<string, object> m_cache; //cache resource loaded

    //check cached 
    public bool Cached(string link)
    {
        return m_cache.ContainsKey(link);
    }

    //get texure from cache
    public Texture GetTexture(string link)
    {
        if (!m_cache.ContainsKey(link))
        {
            return null;
        }
        object texture = m_cache[link];
        if (texture == null)
        {
            return null;
        }
        return (Texture) texture;
    }

    //save resource
    public void Cache(string link, object cache, bool replace = false)
    {
        if (replace) //remove if replace = true;
        {
            m_cache.Remove(link);
        }
        if (m_cache.ContainsKey(link)) //return if contains
        {
            return;
        }
        m_cache.Add(link, cache);
    }
}

public static class ResourceUtils
{
    //call as extension
    public static void LoadTexture(this RawImage rawImage, string link)
    {
        LoadTexture(rawImage, link, true);
    }
    public static void LoadTexture(this RawImage rawImage, string link, bool cache )
    {
        if (string.IsNullOrEmpty(link))
        {
            return;
        }
        Uri uriResult;
        bool result = Uri.TryCreate(link, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        if (!result)
        {
            Texture texture = Resources.Load<Texture>(link);
            if(texture != null)
            {
                rawImage.texture = texture;
            }
            return;
        }
        ResourceHelper.LoadTexture(rawImage, link, cache, false);
    }

    public static void DownloadTextureURL(string url, Action<Texture2D> callback)
    {
        ResourceHelper.DownloadTextureURL(url, callback);
    }
}

//load texture and set to component
public class ResourceHelper : MonoBehaviour
{
    private const int MAX_RETRY = 4;

    public static void LoadTexture(RawImage rawImage, string link, bool cache, bool replace)
    {
        if(rawImage == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(link))
        {
            return;
        }
        //load from cache if already
        if (!replace && ResourceLoader.Api.Cached(link) && cache)
        {
            rawImage.texture = ResourceLoader.Api.GetTexture(link);
            return;
        }
        //load from WWW
        GameObject go = new GameObject
        {
            name = "load_texture"
        };
        DontDestroyOnLoad(go);
        ResourceHelper view = go.AddComponent<ResourceHelper>();
        view.Load(rawImage, link, cache, replace);
    }

    private void Load(RawImage rawImage, string link, bool cache, bool replace)
    {
        StartCoroutine(Wait(rawImage, link, cache, replace));
    }

    private IEnumerator Wait(RawImage rawImage, string link, bool cache, bool replace)
    {
        int retry = 0;
        bool isRetry = false;
        WWW www;
        do
        {
            www = new WWW(link);
            yield return www;
            retry++;
            isRetry = !string.IsNullOrEmpty(www.error) && retry < MAX_RETRY;
            if (isRetry)
            {
                Debug.Log("Download error:" + www.error);
                Debug.Log("Retry download " + retry + ":" + link);
                yield return new WaitForSeconds(0.1f);
            }
        } while (isRetry);

        if (rawImage != null && retry < MAX_RETRY)
        {
            rawImage.texture = www.texture;
            if (cache) //save to cache
            {
                ResourceLoader.Api.Cache(link, www.texture, replace);
            }
        }
        // else
        // {
        //     if (cache) //save to cache
        //     {
        //         ResourceLoader.Api.Cache(link, null, replace);
        //     }
        // }

        Destroy(gameObject); //destroy game object when complete
    }

    public static void DownloadTextureURL(string url, Action<Texture2D> callback)
    {
        GameObject go = new GameObject
        {
            name = "load_texture_url : " + url
        };
        DontDestroyOnLoad(go);
        ResourceHelper view = go.AddComponent<ResourceHelper>();
        view.DownloadTexture(url, callback);
    }

    private void DownloadTexture(string url, Action<Texture2D> callback)
    {
        StartCoroutine(LoadFromWeb(url, callback));
    }

    private IEnumerator LoadFromWeb(string url, Action<Texture2D> callback)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.Send();
        if (wr.isDone)
        {
            callback?.Invoke(texDl.texture);
        }
        else
        {
            callback?.Invoke(null);
        }
        Destroy(gameObject);
    }
}

public class DownloadHelper : MonoBehaviour
{
    public static bool IsUrl(string link)
    {
        Uri uriResult;
        return Uri.TryCreate(link, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public static GameObject DownloadFile(string url, string savePath, Action<string, float, float, float> onUpdate, Action<string> onComplete)
    {
        if (string.IsNullOrEmpty(savePath) || string.IsNullOrEmpty(url) || !IsUrl(url))
        {
            onComplete?.Invoke(url);
            return null;
        }
        GameObject go = new GameObject
        {
            name = "download"
        };
        DontDestroyOnLoad(go);
        DownloadHelperComponent view = go.AddComponent<DownloadHelperComponent>();
        view.Load(url, savePath, onUpdate, onComplete);

        return go;
    }

    private static void DownloadFile(List<string> urls, List<string> paths, int maxThread, Action<string> onComplete, Action onFinish, Action<float,float,float> onUpdate)
    {
        List<string> reaUrls = new List<string>();
        List<string> realPaths = new List<string>();
        for (int i = 0; i < urls.Count; i++)
        {
            string url = urls[i];
            string path = paths[i];
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(url) || !IsUrl(url))
            {
                continue;
            }
            reaUrls.Add(url);
            realPaths.Add(path);
        }

        if (reaUrls == null || reaUrls.Count == 0)
        {
            onFinish?.Invoke();
            return;
        }

        s_onUpdate = onUpdate;
        s_pathProcess.Clear();
        s_totalPath = reaUrls.Count;
        //
        s_urls.AddRange(reaUrls);
        s_paths.AddRange(realPaths);
        if (s_downloading.Count > 0)
        {
            return;
        }
        s_onComplete += onFinish;
        int count = 0;
        for (int i = 0; i < s_urls.Count; i++)
        {
            DownloadFile(s_urls[i], s_paths[i], OnUpdateForPath, (data) => OnComplete(data, onComplete));
            s_downloading.Add(s_urls[i]);
            s_urls.RemoveAt(0);
            s_paths.RemoveAt(0);
            count++;
            i--;
            if (count >= maxThread)
            {
                break;
            }
        }
    }

    private static void OnComplete(string url, Action<string> onComplete)
    {
        onComplete?.Invoke(url);
        s_downloading.Remove(url);
        if (s_downloading.Count < 1)
        {
            s_onComplete?.Invoke();
            s_onComplete = null;
            return;
        }
        if (s_urls.Count < 1)
        {
            return;
        }
        string nextUrl = s_urls[0];
        string nextPath = s_paths[0];
        s_downloading.Add(nextUrl);
        s_urls.RemoveAt(0);
        s_paths.RemoveAt(0);
        DownloadFile(nextUrl, nextPath, OnUpdateForPath, (data) => OnComplete(data, onComplete));
    }

    private static void OnUpdateForPath(string url, float process, float donwloadedSize, float fileSize)
    {
        if (s_pathProcess.ContainsKey(url))
        {
            s_pathProcess[url] = process;
        }
        else
        {
            s_pathProcess.Add(url, process);
        }
        float part = 1f / s_totalPath;
        float realProcess = 0f;
        foreach (KeyValuePair<string, float> child in s_pathProcess)
        {
            realProcess += child.Value * part;
        }
        s_onUpdate?.Invoke(realProcess, donwloadedSize, fileSize);
    }

    private static List<string> s_urls = new List<string>();
    private static List<string> s_paths = new List<string>();
    private static List<string> s_downloading = new List<string>();
    private static Action s_onComplete;
    private static Action<float, float, float> s_onUpdate; //update process for path <percent, downloadedSize, fileSize>
    private static Dictionary<string, float> s_pathProcess = new Dictionary<string, float>(); //path download done
    private static int s_totalPath; //total path
}

public class DownloadHelperComponent : MonoBehaviour
{
    private int MAX_RETRY = 3;

    private string m_url;
    private UnityWebRequest m_www;
    private Action<string, float, float, float> m_onUpdate;
    private float m_fileSize;

    public void Load(string url, string savePath, Action<string, float, float, float> onUpdate, Action<string> onComplete)
    {
        m_onUpdate = onUpdate;
        m_url = url;
        StartCoroutine(Download(url, savePath, onComplete));
    }

    private IEnumerator Download(string url, string savePath, Action<string> onComplete)
    {
        Debug.Log("DownloadHelperComponent - Start download file:" + url);
        int retry = 0;
        bool isRetry = false;

        do
        {
            m_www = UnityWebRequest.Get(url);
            yield return m_www.SendWebRequest();
            retry++;
            isRetry = !string.IsNullOrEmpty(m_www.error) && retry < MAX_RETRY;
            if (isRetry)
            {
                Debug.Log("Error :" + m_www.error);
                Debug.Log("Retry download :" + url);
            }
        } while (isRetry);

        if (retry < MAX_RETRY)
        {
            //save
            byte[] results = m_www.downloadHandler.data;
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllBytes(savePath, results);
        }
        yield return null;
        m_onUpdate?.Invoke(url, m_fileSize, m_fileSize, 1f);
        onComplete?.Invoke(url);
        Debug.Log("download done: " + url);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (m_www == null)
            return;

        if (!m_www.isDone)
        {
            float downloadedMb = (float)m_www.downloadedBytes * Mathf.Pow(10,-6);
            m_onUpdate?.Invoke(m_url, m_www.downloadProgress, downloadedMb, downloadedMb / m_www.downloadProgress);
        }
    }
}

public class AssetBundleHelper
{
    public static AssetBundle LoadFromFile(string path)
    {
        if (s_bundles.ContainsKey(path))
        {
            return s_bundles[path];
        }
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        s_bundles.Add(path, bundle);
        return bundle;
    }

    public static void Unload(string path)
    {
        if (!s_bundles.ContainsKey(path))
        {
            return;
        }
        AssetBundle bundle = s_bundles[path];
        s_bundles.Remove(path);
        bundle.Unload(true);
    }

    public static void UnloadAllAssetBundles()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        s_bundles.Clear();
    }

    private static Dictionary<string, AssetBundle> s_bundles = new Dictionary<string, AssetBundle>();
}