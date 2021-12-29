using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


/// <summary>
/// uGUIのアニメーションをしてみたが、リビルドが走るという事で、本来は避けたい。
/// Canvasの分割か、UIControllerか、、、
/// </summary>
/// 


public class ItemListManager : MonoBehaviour
{
    enum BtnType : int
    {
        Add,
        Remove,
        Buy,
    }
    bool IsFadeIn { get; set; }     //FadeIn中は操作不可とする。

    // view
    [SerializeField] private GameObject _content;      //Itemを入れる親
    [SerializeField] private ItemContent _itemPrefab;    //ItemCell
    [SerializeField] private ItemMaster _master;        //Master
    [SerializeField] private Button[] _buttons = new Button[3];//Add,Remove,Buy

    [SerializeField] private GameObject _inventoryListContent;  // プレイヤーのインベントリ。
    [SerializeField] private InventoryContent _inventoryPrefab;

    [SerializeField] private SelectedTable _selectedTable;  //選択した物を配置する場所。
    private int _selectedItemIndex = -1;

    // データを読み込んで、展開。
    // 順番にアニメーションを再生していきたい。
    [SerializeField] private float _itemFadeInLagTime;   // ItemCellを順番に出す間隔の時間

    private List<ItemContent> _itemListBuf = new List<ItemContent>();   // 表示している物を参照しておく。


    // Start is called before the first frame update
    void Start()
    {
        IsFadeIn = true;
        StartCoroutine(FadeIn());

        Assert.AreEqual(_buttons.Length, 3, "ボタンの数が合わない");
        _buttons[(int)BtnType.Add].onClick.AddListener(OnClickAdd);
        _buttons[(int)BtnType.Remove].onClick.AddListener(OnClickRemove);
        _buttons[(int)BtnType.Buy].onClick.AddListener(OnClickBuy);
    }

    void OnClickAdd()
    {
        var list = _master.items;
        if (list.Count == 0)
        {
            return;
        }

        var item = list[Random.Range(0, list.Count)];
        MakeItemButton(item);

        _buttons[(int)BtnType.Remove].interactable = true;
    }

    void OnClickRemove()
    {
        if (_itemListBuf.Count > 0)
        {
            RemoveItem(_itemListBuf.Count - 1);
        }
    }

    private IEnumerator FadeIn()
    {
        //マスターからデータ取得して、リスト展開。
        //開始、FadeIn実行終了後のコールバックまで設定。
        //リスト展開アニメーション
        foreach (var item in _master.items)
        {
            MakeItemButton(item);

            //if(画面が埋まったらあとは無視)
            yield return new WaitForSeconds(_itemFadeInLagTime);
        }
    }

    void OnClickBuy()
    {
        if (_selectedItemIndex < 0)
        {
            Debug.LogAssertion("Index値が不正");
            return;
        }

        ///  Inventory PlayerData更新完了したら削除。
        //  リクエスト　Player購入可否判定(値段、空き枠)
        //  レスポンスコールバックで データと描画を更新。

        //　今回　PlayerData。
        var item = _itemListBuf[_selectedItemIndex];
        var content = Instantiate(_inventoryPrefab, Vector3.zero, Quaternion.identity, _inventoryListContent.transform);
        content.Init(item.ItemData);

        _buttons[(int)BtnType.Buy].interactable = false;
        _selectedTable.Clear();

        //リストから削除。
        RemoveItem(_selectedItemIndex);
        _selectedItemIndex = -1;
    }

    /// <summary>
    /// リストボタン押された時のコールバック。
    /// </summary>
    void OnContentClickCallback(ItemContent item)
    {
        Assert.IsNotNull(_selectedTable);

        Debug.Log("onCallback" + item.Index + item.name);
        _selectedItemIndex = FindItemContentIndex(item);
        _selectedTable.SetItem(item.ItemData);

        _buttons[(int)BtnType.Buy].interactable = true;
    }

    /// <summary>
    /// アイテムセル用のプレファブを生成してリストへ追加。
    /// </summary>
    /// <returns></returns>
    private void MakeItemButton(ItemData data)
    {
        var content = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _content.transform);
        content.Init(_itemListBuf.Count, data);
        content.SetCallback(OnContentClickCallback);

        _itemListBuf.Add(content);
    }

    private void RemoveItem(int index)
    {
        // ビューとバッファから削除する。
        Destroy(_itemListBuf[index].gameObject);
        _itemListBuf.RemoveAt(index);

        if (_itemListBuf.Count <= 0)
        {
            _buttons[(int)BtnType.Remove].interactable = false;
        }
    }

    private int FindItemContentIndex(ItemContent content)
    {
        for (int i = 0; i < _itemListBuf.Count; i++)
        {
            if(_itemListBuf[i] == content)
            {
                return i;
            }
        }

        return -1;
    }
}
