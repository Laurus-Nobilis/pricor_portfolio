using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PotionUseRect : MonoBehaviour
{
    [SerializeField] ExpItemElement _cellElement;
    [SerializeField] Transform _content;        // リスト追加先

    List<ExpItemElement> _elements = new List<ExpItemElement>();

    //int _uuidSeed = 0;//固有IDを振らなければと思ったけど、設計が無理すぎる。ダメだやり直し。

    public void InitList(List<ItemData> items, Action<ItemData> callback)
    {
        _elements.Clear();
        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in items)
        {
            var cell = Instantiate(_cellElement, Vector3.zero, Quaternion.identity, _content);
            _elements.Add(cell);

            cell.Item = item;

            //TODO: _number.text ="x"+ ItemDataとは別に所持数を持たせる

            cell.Name.text = item.name;

            //TODO: AssetBundle && Adressable
            cell.Image.sprite = Resources.Load<Sprite>("RPGIcon/" + item.asset_id);

            cell.Button.onClick.AddListener(delegate { callback(item); });
        }
    }

    public void RemoveItem(ItemData item)
    {
        ExpItemElement buf = null;
        foreach(var elm in _elements)
        {
            if (elm.Item == item)
            {
                buf = elm;
            }
        }
        Destroy(buf.gameObject);
        _elements.Remove(buf);

    }
}
