using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

// StreamWriter���g���ĕ������ۑ������B

// JsonUtility�̎�
//  �EDictionary�ɑΉ����Ă��Ȃ��B
//  �EProperty�ɑΉ����ĂȂ��B
//  �Einterface�uISerializationCallbackReceiver�v���g���ĉ����ł��鎖�������B

public class LocalSaveData
{
    string _saveFilePath = "/LocalSave/save.json";

    string GetDefaultSavePath() => Path.Combine(Application.dataPath + _saveFilePath);

    /// <summary>
    /// �^����ꂽ�I�u�W�F�N�g��Json�`���ŕۑ�����B
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public async Task<bool> SaveAsync<T>(T t)
    {
        var json = JsonUtility.ToJson(t);

        Debug.Log(json);

        StreamWriter writer = new StreamWriter(GetDefaultSavePath(), false, System.Text.Encoding.UTF8);

        await writer.WriteLineAsync(json).ConfigureAwait(false);
        writer.Flush();
        writer.Close();

        return true;
    }

    /// <summary>
    ///   �ۑ�����json�t�@�C���ǂݍ���<T>�^�Ƃ��ĕԂ��B
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<T> LoadAsync<T>()
    {
        StreamReader reader = new StreamReader(GetDefaultSavePath());
        string json = await reader.ReadToEndAsync().ConfigureAwait(false);
        reader.Close();

        Debug.Log(json);

        return JsonUtility.FromJson<T>(json);
    }


    public void Save<T>(T t)
    {
        var json = JsonUtility.ToJson(t);

        Debug.Log(json);

        StreamWriter writer = new StreamWriter(GetDefaultSavePath(), false, System.Text.Encoding.UTF8);

        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }
    public T Load<T>()
    {
        StreamReader reader = new StreamReader(GetDefaultSavePath());
        string json = reader.ReadToEnd();
        reader.Close();

        Debug.Log("Load json : " + json);

        return JsonUtility.FromJson<T>(json);
    }
}
