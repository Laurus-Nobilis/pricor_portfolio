using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public interface IResponseData
{
    public string Json { get; set; }
}
public class ResponseData : IResponseData
{
    public string Json { get; set; }
}

public class NetworkManager
{
    const int _TIMEOUT = 60;
    const string _BASE_ADDRESS = "http://localhost:49002/PriconneR";

    Queue<INetworkTask> _tasks;

    public NetworkManager()
    {
        Debug.Log("");
    }

    public void Request(INetworkTask task)
    {
        SendRequest(task);
    }
    //TODO:
    public void EnqueueTask(INetworkTask task)
    {
        _tasks.Enqueue(task);
    }
    private void SendRequest(INetworkTask task)
    {
        SendPost(
            task,
            CallbackWebRequestSuccess,  //����
            CallbackWebRequestFailed    //���s
        );
    }

    /// <summary>
    /// Callbacks the www success.
    /// </summary>
    /// <param name="response">Response.</param>
    private void CallbackWebRequestSuccess(INetworkTask task, string response)
    {
        var res = new ResponseData();
        res.Json = response;
        task.Success(res);
    }

    /// <summary>
    /// Callbacks the www failed.
    /// </summary>
    private void CallbackWebRequestFailed(INetworkTask task)
    {
        // json�f�[�^�擾�Ɏ��s����
        Debug.LogError("API ���N�G�X�g���s");

        task.Failed();
    }

    // Www�𗘗p���ă��N�G�X�g����
    private async void SendPost(INetworkTask task, Action<INetworkTask, string> cbkSuccess = null, Action<INetworkTask> cbkFailed = null)
    {
        Debug.Log("Address : " + _BASE_ADDRESS + task.SubAddress);

        // �o��
        WWWForm form = task.CreatePostData();
        UnityWebRequest www = UnityWebRequest.Post(_BASE_ADDRESS + task.SubAddress, form);
        www.timeout = _TIMEOUT;
        try
        {
            var check = await RequestAsync(www);
            Debug.Log(check);
            if (www.error != null)
            {
                //���X�|���X�G���[�̏ꍇ
                Debug.LogError(www.error);
                cbkFailed?.Invoke(task);
            }
            else if (www.isDone)
            {
                // ���N�G�X�g�����̏ꍇ
                Debug.Log($"Success:{www.downloadHandler.text}");
                cbkSuccess?.Invoke(task, www.downloadHandler.text);
            }
        }
        catch (UnityWebRequestException e)
        {
            // ��O�L���b�`
            Debug.LogAssertion(e.Message);
            cbkFailed?.Invoke(task);
        }
    }
    private async UniTask<string> RequestAsync(UnityWebRequest www)
    {
        await www.SendWebRequest();
        return "Check async return.";
    }
}
