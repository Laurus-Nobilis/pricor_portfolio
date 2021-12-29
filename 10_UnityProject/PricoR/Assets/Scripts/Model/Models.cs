using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;

// ��݃f�[�^�ւ̃A�N�Z�X��񋟂���B�i��FUser
// ex) Models.GetUser() �Ƃ����悤�ɁA�f�[�^���擾����B
public class Models
{
    /// <summary>
    /// Static field
    /// </summary>
    static Models _instance = null;
    public static Models Instance
    {
        get
        {
            //Assert.IsNotNull(_instance);

            if (_instance != null)
            {
                return _instance;
            }

            Ready();//!!!:property�ōs���ׂ����낤���H�H�H
            return _instance;
        }
    }
    public static void Ready()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = new Models();
        _instance.Init();
    }

    /// <summary>
    /// Data field.
    /// </summary>
    //
    User _user;


    public void Destroy()
    {
        Debug.LogWarning("TODO: Models.Destroy()");
    }

    public void Clear()
    {
        Debug.LogWarning("TODO: Models.Clear()");
    }

    public void Init()
    {
        //_user = new User();
    }

    // User data.
    public User GetUser()
    {
        return _user;
    }
    public void UpdateUser(string json)
    {
        _user = UserModel.Parse(json);//TODO:�������Ȃ��A�l�X�V�̕����ǂ��BUserModel.Update(_user, json);
#if UNITY_EDITOR
        var user_json = JsonUtility.ToJson(_user);
        Debug.Log($"user dump json : {user_json}");
#endif
    }

    public void SetDebugUser()
    {
        _user = UserModel.CreateDebugUserData();

        //�m�F
        var user_json = JsonUtility.ToJson(_user);
        Debug.Log($"user dump json : {user_json}");
    }
}
