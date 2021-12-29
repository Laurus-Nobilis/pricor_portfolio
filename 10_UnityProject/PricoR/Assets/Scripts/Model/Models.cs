using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;

// 常在データへのアクセスを提供する。（例：User
// ex) Models.GetUser() というように、データを取得する。
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

            Ready();//!!!:propertyで行うべきだろうか？？？
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
        _user = UserModel.Parse(json);//TODO:代入じゃなく、値更新の方が良い。UserModel.Update(_user, json);
#if UNITY_EDITOR
        var user_json = JsonUtility.ToJson(_user);
        Debug.Log($"user dump json : {user_json}");
#endif
    }

    public void SetDebugUser()
    {
        _user = UserModel.CreateDebugUserData();

        //確認
        var user_json = JsonUtility.ToJson(_user);
        Debug.Log($"user dump json : {user_json}");
    }
}
