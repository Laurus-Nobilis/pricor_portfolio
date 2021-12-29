using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContent : MonoBehaviour
{
    //ItemListManager.OnClickContent clickCallback;   //依存関係。単純なパーツだから外部に依存しない方が良い。
    public delegate void OnClickContent(ItemContent itemContet);

    public int Index { get; private set; }
    public ItemData ItemData { get; private set; }
    [SerializeField] private Button _button;
    [SerializeField] private Text _name;
    [SerializeField] private Text _cost;
    private OnClickContent _clickCallback;
 
    public void Init(int index, ItemData data)
    {
        ItemData = data;
        Index = index;
        _name.text = $"{data.name}";
        _cost.text = $"{data.cost}";
        _button.onClick.AddListener(() =>
        {
            _clickCallback(this);
        });
    }

    public void SetCallback(OnClickContent callback)
    {
        _clickCallback = callback;
    }

}
