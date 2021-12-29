using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.Assertions;

//namespace priconner
//{
//using priconner.model;
//public class RequestData
//{
//}


//  ジェネリックを交えた クラス設計は難しい。
//      NetworkTask<Model>とする場合に、
//      NetworkManager{ Queue<NetworkTask<型>> ～}　型指定を迫られる。基底クラスかインターフェースを割り当てられるようにする所だが、、、
//  なるほど、Interfaceに分離しよう。


public interface INetworkTask
{
    public string SubAddress { get; }
    public string RequestJson { get; }
    Action CbkSuccess { get; }
    Action CbkFailed { get; }

    public void Success(IResponseData res);
    public void Failed();
    public WWWForm CreatePostData();
}

public abstract class NetworkTask<Model> : INetworkTask
{
    //protected abstract class RequestData { };//リクエストデータクラスにまとめたい、stringの形で保持したくない。

    public string SubAddress { get; protected set; }
    public string RequestJson { get; protected set; }
    public Action CbkSuccess { get; protected set; }
    public Action CbkFailed { get; protected set; }

    public virtual void Success(IResponseData res)
    {
        throw new NotImplementedException();
    }

    public virtual void Failed()
    {
        throw new NotImplementedException();
    }

    public virtual WWWForm CreatePostData()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// ユーザーに関して、ユーザー作成とログインのAPIは分けると思う。こちらは簡易な物としておいておく。
/// </summary>
public class LoginTask : NetworkTask<IResponseData>
{
    public long UserID { get; private set; } = 0;
    public string UserName { get; private set; } = string.Empty;

    //定数キーワード
    const string KEY_USER_NAME = "USER_NAME";

    public LoginTask(Action cbk_success, Action cbk_failed, long user_id, string user_name)
    {
        SubAddress = "/user/login";
        RequestJson = JsonUtility.ToJson(this);
        CbkSuccess = cbk_success;
        CbkFailed = cbk_failed;

        UserID = user_id;
        UserName = user_name;
    }

    public override void Success(IResponseData res)
    {
        //Assert.IsNotNull(res.Json);
        Debug.Log("login task success. " + res.Json);

        //ユーザーデータ保持
        Models.Instance.UpdateUser(res.Json);

        if (!UserModel.ExistUser())
        {
            Debug.LogWarning($"ユーザーデータ更新 id:{UserModel.ReadUserId()}, name:{UserModel.ReadUserName()}");

            var u = Models.Instance.GetUser();
            
            Debug.LogWarning($"ユーザーデータ更新 id:{u.Id}, name:{u.Name}");

            UserModel.SaveUserAccount(u.Id, u.Name);
        }

        if (CbkSuccess != null)
        {
            CbkSuccess();
        }
    }

    public override void Failed()
    {
        Debug.Log("Login task failed.");

        if (CbkFailed != null)
        {
            CbkFailed();
        }
    }

    /// <summary>
    /// UserID、UserNameを送る。
    /// これが初期値の場合新規ユーザーという想定のものだ。
    /// </summary>
    /// <returns></returns>
    public override WWWForm CreatePostData()
    {
        //PlayerPrefs.GetInt("user_id", 0);
        //PlayerPrefs.GetString("user_name", string.Empty);

        //Assert.IsTrue(UserName != string.Empty);

        var ret = new WWWForm();
        ret.AddField("user_id", UserID.ToString());
        ret.AddField("user_name", UserName);

        return ret;
    }
}
//}
