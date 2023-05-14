using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public BaseScene CurrentScene { get; set; }
    public T GetCurrentSceneT<T>()
        where T : BaseScene
    {
        return CurrentScene as T;
    }

    private void Awake()
    {
#if BUILD
        Cursor.lockState = CursorLockMode.Confined;     //커서모드 컨파인
#endif

        if(instance != null)
        {
            return;
        }
        else
        {
            instance = this;    
        }
        DontDestroyOnLoad(this);
    }

   
#if DEV
    void Update()
    {
        UpdateApplicationQuitCondition();
    }

    void UpdateApplicationQuitCondition()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GotoNextStage();
        }
    }

    public void GotoNextStage()
    {
        if (DataBase.Instance.gameData.stageIDX < SceneNameCont.StageSceneName.Length - 1)
        {
            DataBase.Instance.gameData.stageIDX++;      //If not in lastStage, stageIdx++
        }

        GameManager.Instance.Save_GameData();           //Save GameData - SkillTree
        DataBase.Instance.JsonIOSystem.WriteJson();     //Write GameData.json, InGameData.json
        DataBase.Instance.Delete_InGameData();          //Delete this Stage Data

        SceneSystem.Instance.ChangeLoadingScene(SceneNameCont.StageSceneName[DataBase.Instance.gameData.stageIDX]);     //Load Next Stage
    }
#endif
}
