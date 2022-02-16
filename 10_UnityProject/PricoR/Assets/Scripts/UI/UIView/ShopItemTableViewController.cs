using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ショップを想定したテーブルビュー
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<ShopItemData, ShopItemTableViewCell>
{
    [SerializeField] private string _iconAtlasPath = "Please Resources file path";

    protected override void Awake()
    {
        base.Awake();

        //MARK: ここで呼び出すものではない。　一旦これで。
        //アイコンのスプライトシートに含まれるスプライトシートをキャッシュしておく。
        SpriteSheetManager.LoadAll(_iconAtlasPath);
    }

    protected override void Start()
    {
        base.Start();

        //MARK:基底のdelgete型メンバーに入れるのは分かりにくい。イベント登録か、Rxでもよいかもしれない。
        _updateCellDelegate = (cell, data) => {
            //cell.UpdateContent(_iconAtlasPath, data);
            cell.UpdateContent(data);
        };

        //リスト項目のデータを読み込む
        LoadData();
    }

    // リスト項目に対応するセルの高さを返すMethod
    protected override float CellHeightAtIndex(int index)
    {
        if (index >= 0 && index <= _tableData.Count - 1)
        {
            //価格帯によってセルのサイズを変える
            int high_price = 700;
            int middle_price = 300;
            if (_tableData[index]._price >= high_price)
            {
                return 240.0f;
            }
            if (_tableData[index]._price >= middle_price)
            {
                return 160.0f;
            }
        }
        return 128.0f;
    }

    // リスト項目のデータを読み込むMethod
    private void LoadData()
    {
        //一旦、ハードコードで作る。//本来、マスタから取得する。
        //<スプライトシート>　Resources/Sprites/RpgItems1
        _tableData = new List<ShopItemData>() {
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_0", _name = "beginer sword", _price=100, _description="安い剣" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_1", _name = "s16 sword", _price=130, _description="短剣" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_2", _name = "red sword", _price=200, _description="レッド剣" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_3", _name = "generic sword", _price=250, _description="汎用剣" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_4", _name = "sword", _price=360, _description="長巻" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_5", _name = "pp sword", _price=470, _description="ぴょん" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_6", _name = "sword", _price=580, _description="さすまた" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_7", _name = "Xsword", _price=690, _description="クロス剣" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_8", _name = "cold sword", _price=790, _description="透神剣" },
            new ShopItemData{_spriteSheetName = _iconAtlasPath, _iconName ="RpgItems1_9", _name = "gru sword", _price=890, _description="グルカ" },
        };
    }
}
