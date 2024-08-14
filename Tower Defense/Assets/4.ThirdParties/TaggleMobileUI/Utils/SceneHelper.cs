using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    private class SceneInfo
    {
        public Queue<string> scenes;
        public Action onBegin;
        public Action onFinish;
        public float delayFinish;

        public SceneInfo()
        {
            scenes = new Queue<string>();
        }
    }
    
    private static SceneHelper m_api;
    private static SceneHelper api
    {
        get
        {
            if (m_api == null)
            {
                GameObject go = new GameObject();
                DontDestroyOnLoad(go);
                go.name = "LoadSceneHelper";
                m_api = go.AddComponent<SceneHelper>();
                m_api.Init();
            }

            return m_api;
        }
    }

    private static string m_currentScene;

    private Queue<SceneInfo> m_queueLoad; //queue scene need load
    private Queue<string> m_queueUnload; //queue scene need unload
    private bool m_load;
    private bool m_unload;

    //cache last state
    private List<string> m_lastState;

    //load scene additive, other loaded scene will be unload
    public static void LoadScene(params string[] scenes)
    {
        LoadSceneAdditive(true, null, scenes);
    }

    //load scene additive with callback, other loaded scene will be unload
    public static void LoadScene(Action callback, params string[] scenes)
    {
        LoadSceneAdditive(true, callback, scenes);
    }

    //load scene additive, other loaded scene will not be unload
    public static void LoadSceneAdditive(params string[] scenes)
    {
        LoadSceneAdditive(false, null, scenes);
    }

    //load scene additive with callback, other loaded scene will not be unload
    public static void LoadSceneAdditive(Action callback, params string[] scenes)
    {
        LoadSceneAdditive(false, callback, scenes);
    }

    public static void LoadSceneAdditive(bool forceUnloadAll, Action callback, params string[] scenes)
    {
        if(scenes.All(x => !SceneHelper.IsSceneExist(x)))
            return;
        
        if(forceUnloadAll)
        {
            UnloadAllScenes(
                callback: () => {
                    DoLoadSceneAdditive(
                        onBegin: null,
                        onFinish: callback, 0,
                        scenes
                    );
                },
                ignoreScenes: scenes
            );
        }
        else
        {
            DoLoadSceneAdditive(
                onBegin: null,
                onFinish: callback, 0,
                scenes
            );
        }
    }

    private static void DoLoadSceneAdditive(Action onBegin, Action onFinish, float delayFinish, params string[] scenes)
    {
        SceneInfo info = new SceneInfo
        {
            onBegin = onBegin,
            onFinish = onFinish,
            delayFinish = delayFinish
        };
        foreach (string s in scenes)
        {
            if(!IsSceneExist(s))
                continue;

            if (!CheckLoaded(s))
            {
                info.scenes.Enqueue(s);
            }
            else
            {
                Debug.Log("Scene loaded:" + s);
            }
            m_currentScene = s;
        }
        //
        api.m_queueLoad.Enqueue(info);
        //
        if (!api.m_load && api.m_queueLoad.Count > 0) //start coroutine if not start
        {
            api.StartCoroutine(api.Load());
        }
    }

    //unload scene
    public static void UnloadScene(params string[] scene)
    {
        DoUnloadSceneAdditive(null, false, scene);
    }

    //unload scene
    public static void DoUnloadSceneAdditive(Action callback, bool cache, params string[] scene)
    {
        foreach (string s in scene)
        {
            if (CheckLoaded(s))
            {
                api.m_queueUnload.Enqueue(s);
                if (cache)
                {
                    api.m_lastState.Add(s);
                }
            }
            else
            {
                Debug.Log("Scene not loaded:" + s);
            }
        }

        if (!api.m_unload && m_api.m_queueUnload.Count > 0) //start coroutine if not start
        {
            api.StartCoroutine(api.Unload(callback));
        }
        else
        {
            callback?.Invoke();
        }
    }

    public static void UnloadAllScenes(params string[] ignoreScenes)
    {
        UnloadAllScenes(null, ignoreScenes);
    }

    public static void UnloadAllScenes(Action callback, params string[] ignoreScenes)
    {
        List<string> scenes = new List<string>();
        for (int i = SceneManager.sceneCount - 1; i > -1; i--)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (ignoreScenes.Contains(s.name) || s == SceneManager.GetSceneByBuildIndex(0))
            {
                continue;
            }
            scenes.Add(s.name);
        }
        api.m_lastState.Clear();
        DoUnloadSceneAdditive(callback, true, scenes.ToArray());
    }

    public static void BackState()
    {
        LoadScene(m_api.m_lastState.ToArray());
        api.m_lastState.Clear();
    }

    public static bool CheckLoaded(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }

    /// <summary>
    /// Returns true if the scene 'name' exists and is in your Build settings, false otherwise
    /// </summary>
    public static bool IsSceneExist(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }

    private void Init()
    {
        m_queueLoad = new Queue<SceneInfo>();
        m_queueUnload = new Queue<string>();
        m_load = false;
        m_unload = false;
        m_lastState = new List<string>();
    }

    //coroutine load scene
    private IEnumerator Load()
    {
        m_load = true;
        while (m_load)
        {
            SceneInfo scene = m_queueLoad.Dequeue();
            scene.onBegin?.Invoke();
            while (scene.scenes.Count > 0)
            {
                AsyncOperation sync = SceneManager.LoadSceneAsync(scene.scenes.Dequeue(), LoadSceneMode.Additive);
                yield return sync;
            }
            if (scene.delayFinish > 0)
            {
                yield return new WaitForSeconds(scene.delayFinish);
            }

            scene.onFinish?.Invoke();
            //
            if (m_queueLoad.Count < 1) //stop if queue empty
            {
                m_load = false;
            }
        }
    }

    //coroutine unload
    private IEnumerator Unload(Action callback)
    {
        m_unload = true;
        while (m_unload)
        {
            Scene scene = SceneManager.GetSceneByName(m_queueUnload.Dequeue());
            if(scene != null && scene.isLoaded) 
            {
                yield return SceneManager.UnloadSceneAsync(scene);
            }

            if (m_queueUnload.Count < 1) //stop if queue empty
            {
                m_unload = false;
            }
        }

        yield return new WaitForEndOfFrame();
        callback?.Invoke();
    }

    public static void SetActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
}
