using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelUpMenuRect : MonoBehaviour
{
    [SerializeField] ExpItemElement _cellElement;
    [SerializeField] Transform _content;        // ���X�g�ǉ���

    List<ExpItemElement> _elements = new List<ExpItemElement>();

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

            //TODO: _number.text ="x"+ ItemData�Ƃ͕ʂɏ���������������

            cell.Name.text = item.name;

            //TODO: AssetBundle && Adressable
            cell.Image.sprite = Resources.Load<Sprite>("RPGIcon/" + item.asset_id);

            cell.Button.onClick.AddListener(delegate { callback(item); });
        }
    }



}
