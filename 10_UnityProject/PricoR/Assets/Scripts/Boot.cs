using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBoot()
    {
        //必ず配置するオブジェクト用のシーンを読み込む
        //  (Prefabを出してDontDestroyもできるかな。でも増やしやすいこっちで。)
        string managerSceneName = "ManagerScene";
        if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
        {
            SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
        }
    }
}
