using System.Collections;
using Services.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DTControl
{
    private static DTControl m_api;
    public static DTControl api
    {
        get
        {
            return m_api ??= new DTControl();
        }
    }

    public void Init()
    {
        Debug.Log("Init mini app main control");

        Helpers.GetWaitForSeconds(5f);
        LoadGameplayScene();
    }

    private void LoadGameplayScene()
    {
        SceneManager.LoadSceneAsync(DTConstant.SCENE_GAMEPLAY, LoadSceneMode.Additive);
    }
}
