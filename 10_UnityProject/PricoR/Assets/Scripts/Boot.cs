using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBoot()
    {
        //�K���z�u����I�u�W�F�N�g�p�̃V�[����ǂݍ���
        //  (Prefab���o����DontDestroy���ł��邩�ȁB�ł����₵�₷���������ŁB)
        string managerSceneName = "ManagerScene";
        if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
        {
            SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
        }
    }
}
