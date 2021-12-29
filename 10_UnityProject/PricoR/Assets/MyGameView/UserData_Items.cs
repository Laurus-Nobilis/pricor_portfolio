using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData_Items : MonoBehaviour
{
    const string SaveKey = "OwnedItems";
    int curCurrency;
    List<int> ownedItems = new List<int>();

    void Dump()
    {
        Debug.Log("Dump list cnt : "+ ownedItems.Count);
        foreach (var o in ownedItems)
        {
            Debug.Log($"item id : {o}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ownedItems.Count == 0)
        { ownedItems.Add(2); }
        Dump();

        Invoke("LocalSave", 2);
        Invoke("LocalLoad", 5);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void LocalSave()
    {
        //Debug.Log("Save" + JsonUtility.ToJson(ownedItems).ToString());

        //PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(ownedItems));


        PlayerPrefs.SetString(SaveKey, "A");
    }

    void LocalLoad()
    {
        var a = PlayerPrefs.GetString(SaveKey, string.Empty);
        Debug.Log(a);
        //var json = PlayerPrefs.GetString(SaveKey, string.Empty);
        //ownedItems = JsonUtility.FromJson<List<int>>(json);
        //Debug.Log("Load" + ownedItems.ToString());
        //foreach (var o in ownedItems)
        //{
        //    Debug.Log($"item id : {o}");
        //}
    }
}
