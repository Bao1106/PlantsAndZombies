using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitView : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        
        SceneManager.LoadSceneAsync(DTConstant.SCENE_LOAD_FIRST, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(DTConstant.SCENE_INIT);
    }
}
