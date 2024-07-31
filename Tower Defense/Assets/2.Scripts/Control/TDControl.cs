using UnityEngine;
using UnityEngine.SceneManagement;

public class TDControl
{
    private static TDControl m_api;
    public static TDControl api
    {
        get
        {
            return m_api ??= new TDControl();
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
        //Init main control
        TDGameplayMainControl.api = new TDGameplayMainControl();
        TDEnemyPathMainControl.api = new TDEnemyPathMainControl();
        TDTowerMainControl.api = new TDTowerMainControl();
        
        //Init sub control
        TDEnemyPathControl.api = new TDEnemyPathControl();
        TDPlaceTowerControl.api = new TDPlaceTowerControl();
        TDTowerFactoryControl.api = new TDTowerFactoryControl();
        TDUserInputControl.api = new TDUserInputControl();
        
        TDInitializeModel.api = new TDInitializeModel();
    }
    
    private void LoadGameplayScene()
    {
        SceneManager.LoadSceneAsync(TDConstant.SCENE_GAMEPLAY, LoadSceneMode.Additive);
    }
}
