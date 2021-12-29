using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemMaster", menuName = "SO/ItemMaster", order = 0)]
public class ItemMaster : ScriptableObject, ISerializationCallbackReceiver
{
    [FormerlySerializedAs("item list")]
    public List<ItemData> items;

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Debug.Log("After　リストカウント : " + items.Count);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        //Debug.Log("Before　リストカウント : " + items.Count);
        HashSet<int> tmp = new HashSet<int>();
        foreach(var i in items.Select<ItemData, int>(x => x.id))
        {
            if (!tmp.Add(i))
            {
                throw new Exception();
            }
        }
    }
}
