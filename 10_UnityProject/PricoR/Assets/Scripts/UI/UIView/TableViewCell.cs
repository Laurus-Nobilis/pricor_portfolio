using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// UI TableViewのCell
/// Cellの汎用性を上げるためにジェネリックとする。
/// ジェネリック型<T>には データタイプ を指定する。
/// 
/// 課題：
/// 縦横の両方向に対応すの事も考えたいな。
/// 例えば　Height は Lengthにするのかとかそういう所から。
/// </summary>
public abstract class TableViewCell<T> : ViewController
{
    //セルの内容を更新するメソッド
    public abstract void UpdateContent(T itemData);

    //セルに対応するリスト項目のインデックスを保持
    public int DataIndex { get; set; }

    //セルの高さを取得、設定するProperty
    public float Height
    {
        get { return CachedRectTransform.sizeDelta.y; }
        set
        {
            var sizeDelta = CachedRectTransform.sizeDelta;
            sizeDelta.y = value;
            CachedRectTransform.sizeDelta = sizeDelta;
        }
    }

    //セルの上端の位置を取得、設定するProperty
    public Vector2 Top
    {
        get
        {
            var corners = new Vector3[4];
            //4頂点を取得する。順番は左下から時計回りに格納される。
            CachedRectTransform.GetLocalCorners(corners);
            //corners[1]==左上。
            //AnchoredPositionに高さを足した位置。
            return CachedRectTransform.anchoredPosition + new Vector2(0.0f, corners[1].y);
        }
        set
        {
            var corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            CachedRectTransform.anchoredPosition = value - new Vector2(0.0f, corners[1].y);
        }
    }

    //セルの下端の位置を取得、設定するProperty
    public Vector2 Bottom
    {
        get
        {
            var corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            return CachedRectTransform.anchoredPosition + new Vector2(0.0f, corners[3].y);
        }
        set
        {
            var corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            CachedRectTransform.anchoredPosition = value - new Vector2(0.0f, corners[3].y);
        }
    }
}
