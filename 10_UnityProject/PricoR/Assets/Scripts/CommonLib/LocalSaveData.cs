using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

// StreamWriterを使って文字列を保存するよ。

// JsonUtilityの事
//  ・Dictionaryに対応していない。
//  ・Propertyに対応してない。
//  ・interface「ISerializationCallbackReceiver」を使って解決できる事が多い。

public class LocalSaveData
{
    string _saveFilePath = "/LocalSave/save.json";

    string GetDefaultSavePath() => Path.Combine(Application.dataPath + _saveFilePath);

    /// <summary>
    /// 与えられたオブジェクトをJson形式で保存する。
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
    ///   保存したjsonファイル読み込み<T>型として返す。
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
