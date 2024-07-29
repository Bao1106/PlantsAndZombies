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
        InitOtherControl();
        
        LoadGameplayScene();
    }

    private void InitOtherControl()
    {
        TowerMainControl.api = new TowerMainControl();
        PlaceTowerControl.api = new PlaceTowerControl();
        TowerFactoryControl.api = new TowerFactoryControl();
        UserInputControl.api = new UserInputControl();
    }
    
    private void LoadGameplayScene()
    {
        SceneManager.LoadSceneAsync(DTConstant.SCENE_GAMEPLAY, LoadSceneMode.Additive);
    }
}
