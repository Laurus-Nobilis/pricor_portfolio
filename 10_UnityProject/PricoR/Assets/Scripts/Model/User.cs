using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserModel : Model<User>
{
    const string user_id = "user_id";
    const string user_name = "user_name";

    public static User Parse(string json)
    {
        //レスポンスサンプル
        //{"id":43,"name":"SAOYAMA","rank":1,"exp":0,"lastlogin":"2021-12-20T06:42:18+00:00","created":"2021-12-20T06:42:18+00:00","items":null,"units":null}

        var dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
        var u = new User();
        u.Id = (long)dict["id"];
        u.Name = (string)dict["name"];
        u.Rank = Convert.ToInt32(dict["rank"]);
        u.Exp = Convert.ToInt32(dict["exp"]);

        return u;
    }


    public static User CreateDebugUserData()
    {
        //レスポンスサンプル
        //{"id":43,"name":"SAOYAMA","rank":1,"exp":0,"lastlogin":"2021-12-20T06:42:18+00:00","created":"2021-12-20T06:42:18+00:00","items":null,"units":null}

        var u = new User();
        u.Id = 0;
        u.Name = "テストマン";
        u.Rank = 1;
        u.Exp = 9;

        return u;
    }

    /// <summary>
    /// 既存ユーザーかどうか判定する。
    /// ユーザーデータの有無をチェックする。生データは改ざんされるから本来はアウト。
    /// </summary>
    /// <returns></returns>
    public static bool ExistUser()
    {
        Debug.Log($"###ユーザーデータ###  {ReadUserId()}, {ReadUserName()}");
        return (ReadUserName() != string.Empty && ReadUserId() != 0);
    }

    public static long ReadUserId() => Convert.ToInt64(PlayerPrefs.GetString(user_id, "0"));
    /// <summary>
    /// PlayerPrefsに保存された User name を取得する。defaultはEmpty。
    /// </summary>
    /// <returns></returns>
    public static string ReadUserName() => PlayerPrefs.GetString(user_name, string.Empty);
    public static void SaveUserAccount(long id, string name)
    {
        PlayerPrefs.SetString(user_id, id.ToString());
        PlayerPrefs.SetString(user_name, name);
        PlayerPrefs.Save();
    }
}

public class User
{
    [SerializeField, HideInInspector] long _id;
    [SerializeField, HideInInspector] string _name;
    [SerializeField, HideInInspector] int _rank;
    [SerializeField, HideInInspector] int _exp;
    [SerializeField, HideInInspector] List<Unit> _units;
    [SerializeField, HideInInspector] List<Item> _items;
    [SerializeField, HideInInspector] List<Equipment> _equipments;
    [SerializeField, HideInInspector] MainUnit _mainUnit;

    // set はModelクラス以外ではさせたくないが、一旦後回しだ。
    public long Id { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public int Rank { get => _rank; set => _rank = value; }
    public int Exp { get => _exp; set => _exp = value; }
    public List<Unit> Units { get => _units; set => _units = value; }
    public List<Item> Items { get => _items; set => _items = value; }
    public List<Equipment> Equipments { get => _equipments; set => _equipments = value; }
    public MainUnit MainUnit { get => _mainUnit; set => _mainUnit = value; }

    public User() { }
    public User(long id, string name, int rank, int exp, ref List<Unit> units, ref List<Item> items, ref List<Equipment> equip, MainUnit mainunit)
    {
        Id = id;
        Name = name;
        Rank = rank;
        Exp = exp;
        Units = units;
        Items = items;
        Equipments = equip;
        MainUnit = mainunit;
    }
}

