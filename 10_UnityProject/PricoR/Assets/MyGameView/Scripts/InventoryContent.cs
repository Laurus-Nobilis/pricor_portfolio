using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Text _name;
    [SerializeField] Text _cost;
    public void Init(ItemData item)
    {
        _icon.sprite = Resources.Load<Sprite>($"RPGIcon/{item.asset_id}");
        _name.text = item.name;
        _cost.text = $"{item.cost}";
    }
}
