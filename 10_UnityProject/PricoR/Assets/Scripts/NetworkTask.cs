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


//  �W�F�l���b�N�������� �N���X�݌v�͓���B
//      NetworkTask<Model>�Ƃ���ꍇ�ɁA
//      NetworkManager{ Queue<NetworkTask<�^>> �`}�@�^�w��𔗂���B���N���X���C���^�[�t�F�[�X�����蓖�Ă���悤�ɂ��鏊�����A�A�A
//  �Ȃ�قǁAInterface�ɕ������悤�B


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
    //protected abstract class RequestData { };//���N�G�X�g�f�[�^�N���X�ɂ܂Ƃ߂����Astring�̌`�ŕێ��������Ȃ��B

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
/// ���[�U�[�Ɋւ��āA���[�U�[�쐬�ƃ��O�C����API�͕�����Ǝv���B������͊ȈՂȕ��Ƃ��Ă����Ă����B
/// </summary>
public class LoginTask : NetworkTask<IResponseData>
{
    public long UserID { get; private set; } = 0;
    public string UserName { get; private set; } = string.Empty;

    //�萔�L�[���[�h
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

        //���[�U�[�f�[�^�ێ�
        Models.Instance.UpdateUser(res.Json);

        if (!UserModel.ExistUser())
        {
            Debug.LogWarning($"���[�U�[�f�[�^�X�V id:{UserModel.ReadUserId()}, name:{UserModel.ReadUserName()}");

            var u = Models.Instance.GetUser();
            
            Debug.LogWarning($"���[�U�[�f�[�^�X�V id:{u.Id}, name:{u.Name}");

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
    /// UserID�AUserName�𑗂�B
    /// ���ꂪ�����l�̏ꍇ�V�K���[�U�[�Ƃ����z��̂��̂��B
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
