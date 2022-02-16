using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


/// <summary>
/// 「セル」に表示するデータ
/// 販売アイテムを想定した物。
/// </summary>
public class ShopItemData
{
    public string _spriteSheetName; //アイコン名
    public string _iconName; //アイコン名
    public string _name; //アイテム名
    public int _price;  //価格
    public string _description;  //説明
}

/// <summary>
/// ショップを想定した「セル」です
/// </summary>
public class ShopItemTableViewCell : TableViewCell<ShopItemData>
{
    [SerializeField] private Image _iconImage;  //アイコン用イメージ
    [SerializeField] private TextMeshProUGUI _nameLabel;   //アイテム名ラベル
    [SerializeField] private TextMeshProUGUI _priceLabel;  //価格ラベル
    
    //TODO:このMethodは設計の間違い。基底クラスに不要かも知れない。
    //セルの内容を更新する。
    public override void UpdateContent(ShopItemData itemData)
    {
        _nameLabel.text = itemData._name;
        _priceLabel.text = itemData._price.ToString();

        _iconImage.sprite = SpriteSheetManager.GetSpriteByName(itemData._spriteSheetName, itemData._iconName);
    }
}
