using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ExpUpアイテムのリストビューに入れるもの。　複雑な事はさせない。
/// </summary>
public class ExpItemElement : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Text _number;
    [SerializeField] Text _name;
    [SerializeField] Button _button;
    public Image Image { get => _image; set => _image = value; }
    public Text Number { get => _number; set => _number = value; }
    public Text Name { get => _name; set => _name = value; }
    public Button Button { get => _button; set => _button = value; }
    public ItemData Item { get; set; }  //参照をセットしておく。
    
}
