using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ドロップ可能エリア。
/// ・ドロップ可能になった事をハイライトなどで伝える
/// ・ドロップされたらそのオブジェクトを表示する。
/// ・
/// </summary>
public class Droppable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconImage;   //ドロップエリアに表示しているアイコン
    [SerializeField] private Color _highlightedColor;    //ドロップエリアに表示しているアイコンのハイライト色
    private Color _normalColor;  //ドロップエリア表示のアイコンの元の色を保持

    /// <summary>
    /// ドロップされた場合
    /// ドロップされたオブジェクト情報を取得して表示する
    /// </summary>
    /// <param name="eventData"></param>
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        //ドラッグしていたアイコンのImageコンポーネントを取得する
        var dropImg = eventData.pointerDrag.GetComponent<Image>();

        //ドロップエリアに表示しているアイコンのスプライトを
        //ドロップされたアイコンと同じスプライトに変更して
        //ハイライトなどを元に戻す。
        _iconImage.sprite = dropImg.sprite;
        _iconImage.color = _normalColor;
    }

    /// <summary>
    /// ドラッグ中にエリアに入ったらハイライト演出する。
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.dragging)
        {
            _iconImage.color = _highlightedColor;
        }
    }

    /// <summary>
    /// エリアから抜けたら、ハイライトなどを元に戻す。
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if(eventData.dragging)
        {
            _iconImage.color = _normalColor;
        }
    }

    private void Start()
    {
        //アイコンの下の色を保存
        _normalColor = _iconImage.color;
    }
}
