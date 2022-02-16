using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// テーブルビュー基底クラス。：ジェネリック
/// セルを使い回し、表示領域より外にあるものは持たない・作らない。
/// </summary>
public abstract class TableViewController<T, TableViewCell> : ViewController
{
    protected List<T> _tableData = new List<T>();   //リスト項目のデータを保持
    [SerializeField] private RectOffset _padding;   //スクロールさせる内容のパティング
    [SerializeField] private float _spacingHeight = 4.0f;   //各セルの間隔

    protected Action<TableViewCell<T>, T> _updateCellDelegate;//cellを更新するデリゲートをサブクラスから設定する；

    //ScrollRectをキャッシュ
    private ScrollRect _cachedScrollRect;
    public ScrollRect CachedScrollRect
    {
        get
        {
            if (_cachedScrollRect == null)
            {
                _cachedScrollRect = GetComponent<ScrollRect>();
            }
            return _cachedScrollRect;
        }
    }

    //Awake
    //（abstractを検討した所サブクラスで必ずAwakeする必要が無いのでvirtualとする。サブからbase.Awakeを呼び出すというルールになる。）
    protected virtual void Awake()
    {
    }

    //リスト項目に対応するセルの高さを取得するMethod。
    protected abstract float CellHeightAtIndex(int index);

    //スクロールさせる内容全体の高さを更新するメソッド。
    protected void UpdateContentSize()
    {
        //スクロールさせる内容全体の高さを算出する
        float contentHeight = 0.0f;
        for (int i = 0; i < _tableData.Count; i++)
        {
            contentHeight += CellHeightAtIndex(i);
            if (i > 0)
            {
                contentHeight += _spacingHeight;
            }
        }

        //スクロールさせる内容の高さを ScrollRect に設定する。
        var sizeDelta = CachedScrollRect.content.sizeDelta;
        sizeDelta.y = _padding.top + contentHeight + _padding.bottom;
        CachedScrollRect.content.sizeDelta = sizeDelta;
    }

    //
    //  -------------------------
    //

    #region セルの更新
    [SerializeField] private GameObject _cellBase;  //コピー元のセル
    private LinkedList<TableViewCell<T>> _cells = new LinkedList<TableViewCell<T>>();   //セルを保持する

    //Start 派生クラスから呼び出させる。
    protected virtual void Start()
    {
        //コピー元のセルは非アクティブ化する。
        _cellBase.SetActive(false);

        //ScrollRect の OnValueChange イベントリスナーを設定する。
        CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);

        //Start()の時点で呼び出した場合、(多分)ScrollRectのレイアウト処理が完了してなくてセル配置が失敗するため、初回１度LateUpdateで行わせる。
        CachedScrollRect.LateUpdateAsObservable()
            .Take(1)
            .Subscribe(x => UpdateContents()).AddTo(this);
    }

    /// <summary>
    /// セルを作成するメソッド。
    /// 設定したCellBaseを複製する。
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private TableViewCell<T> CreateCellForIndex(int index)
    {
        //コピー元のセルから新しいセルを作成する。
        //GameObjectからCellコンポーネントを取得して更新していく。
        var obj = Instantiate(_cellBase) as GameObject;
        TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

        //親要素の付け替えを行うとスケールやサイズが失われるため、変数に保持しておく
        var localPosition = cell.transform.localPosition;
        var scale = cell.transform.localScale;
        var sizeDelta = cell.CachedRectTransform.sizeDelta;
        var offsetMin = cell.CachedRectTransform.offsetMin;
        var offsetMax = cell.CachedRectTransform.offsetMax;

        //親要素の付け替えを行い、元のパラメーターに戻す。
        cell.transform.SetParent(_cellBase.transform.parent);
        //セルのスケールやサイズを設定する
        cell.transform.localPosition = localPosition;
        cell.transform.localScale = scale;
        cell.CachedRectTransform.sizeDelta = sizeDelta;
        cell.CachedRectTransform.offsetMin = offsetMin;
        cell.CachedRectTransform.offsetMax = offsetMax;

        //指定したインデックスのリスト項目に対応するセルとして内容を更新する
        UpdateCellForIndex(cell, index);

        _cells.AddLast(cell);

        return cell;
    }

    /// <summary>
    /// セルの内容を更新するメソッド
    /// </summary>
    /// <param name="cell">更新するセル</param>
    /// <param name="index">更新が必要になったセルのインデックス</param>
    private void UpdateCellForIndex(TableViewCell<T> cell, int index)
    {
        //セルに対応するリスト項目のインデックスを設定する。
        cell.DataIndex = index;

        if (cell.DataIndex >= 0 && cell.DataIndex <= _tableData.Count - 1)
        {
            //セルに対応するリスト項目があれば、セルを「アクティブ、内容、高さ」を設定する。
            cell.gameObject.SetActive(true);
            //MARK: セルの更新処理についてスッキリさせるために、DataSorce的なクラスをさらに追加しても良い。
            //cell.UpdateContent(_tableData[cell.DataIndex]);
            _updateCellDelegate?.Invoke(cell, _tableData[cell.DataIndex]);
            cell.Height = CellHeightAtIndex(cell.DataIndex);
        }
        else
        {
            //セルに対応するリスト項目が無かったら、セルを非アクティブにして表示しない。
            cell.gameObject.SetActive(false);
        }
    }
    #endregion  END セルの更新


    #region _visibleRect 表示可否判定用
    private Rect _viewportRect;  //セルを表示するViewportの矩形
    [SerializeField] private RectOffset _visibleRectPadding;    //_visibleRectのPadding

    private float ContentPosisionDelta { get { return CachedScrollRect.content.localPosition.y; } }
    private float ViewportTop { get { return 0; } }
    private float ViewportBottom { get { return _viewportRect.y; } }

    //_visibleRectを更新する
    private void UpdateVisibleRect()
    {
        //2021 ScrollRectにおいて比較するのは ViewportとContent である。
        // viewportは左上原点(0, 0)で、高さは負数（例, -350）となる。
        // contentが移動するとき、
        //      例：　初期位置（0, 0）で、上にスクロールしていくと、Y値は増加していく。(0 => 1000)
        // 
        //　上端＝viewport(0) vs content(0~Length.Y)　＝　contentのY値そのものである。
        //  下端＝viwport.height(350) vs content　＝　viewport.height + content.y
        //
        //  上記とセルの下端・上端を比較する。

        _viewportRect = CachedScrollRect.viewport.rect;

        //位置は、スクロールさせる内容の基準点からの相対位置
        //_visibleRect.x = CachedScrollRect.content.anchoredPosition.x + _visibleRectPadding.left;
        //_visibleRect.y = CachedScrollRect.content.anchoredPosition.y + _visibleRectPadding.top;

        ////サイズは、スクロールビューのサイズ+パティング
        //_visibleRect.width = CachedRectTransform.rect.width + _visibleRectPadding.left + _visibleRectPadding.right;
        //_visibleRect.height = CachedRectTransform.rect.height + _visibleRectPadding.top + _visibleRectPadding.bottom;

        //Debug.Log($"^^^^^^^^^^^^^^^");
        //Debug.Log($"_visibleRect : {_visibleRect.ToString()}");
        //Debug.Log($"  content : {CachedScrollRect.content.rect.ToString()}");
        //Debug.Log($"position y:{CachedScrollRect.content.rect.y .ToString()}");
        //Debug.Log($"localposition y:{CachedScrollRect.content.localPosition.y .ToString()}");
        //Yスクロールして動いたのはcontent.localPosition.yだけ
    }
    #endregion END _visibleRect 表示可否判定用


    /// <summary>
    /// テーブルビューの表示内容を更新する
    /// セルの位置は隣接セルから相対的に決定する。
    /// </summary>
    protected void UpdateContents()
    {
        UpdateContentSize();    //スクロールさせる内容のサイズを更新する
        UpdateVisibleRect();

        //セル作成してあるか？
        if (_cells.Count < 1)
        {
            // 最初のセルを作る。
            //作成前でセルが1つもない時点、
            //_visibleRectの範囲に入る最初のリスト項目を探して、それに対応するセルを作成する
            var cellTop = new Vector2(0.0f, -_padding.top);
            var content = CachedScrollRect.content;
            for (int i = 0; i < _tableData.Count; i++)
            {
                float cellHeight = CellHeightAtIndex(i);
                var cellBottom = cellTop + new Vector2(0.0f, -cellHeight);
                //セルの上端と、viewport+contentの下端を比較するのじゃ。
                //セルの下端と、viewport+contentの上端を比較するのじゃ。
                //CheckLog:
                //Debug.Log($"Top     :{cellTop.y} + {content.localPosition.y} <= 0 && {cellTop.y} + {content.localPosition.y} >= {_visibleRect.y}");
                //Debug.Log($"Bottom  :{cellBottom.y} + {content.localPosition.y} <= 0 && {cellBottom.y} + {content.localPosition.y} >= {_visibleRect.y}");
                if (
                    (cellTop.y + content.localPosition.y <= 0 && cellTop.y + content.localPosition.y >= _viewportRect.y)
                    || (cellBottom.y + content.localPosition.y <= 0 && cellBottom.y + content.localPosition.y >= _viewportRect.y)
                    )
                {
                    TableViewCell<T> cell = CreateCellForIndex(i);
                    cell.Top = cellTop;
                    break;
                }
                cellTop = cellBottom + new Vector2(0.0f, _spacingHeight);
            }

            //_visibleRectの範囲内に空きがあればセルを作成する
            FillVisibleRectWithCells();
        }
        else
        {
            //既存セルの先頭のセルから順に対応するリスト項目のインデックスを設定し直し、位置と内容を更新する。
            LinkedListNode<TableViewCell<T>> node = _cells.First;
            UpdateCellForIndex(node.Value, node.Value.DataIndex);
            node = node.Next;
            while (node != null)
            {
                UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);
                node.Value.Top = node.Previous.Value.Bottom + new Vector2(0.0f, -_spacingHeight);
                node = node.Next;
            }

            //_visibleRectの範囲内に空きがあればセルを作成する
            FillVisibleRectWithCells();
        }
    }

    // _visibleRectの範囲内に表示される分のセルを作成するメソッド
    private void FillVisibleRectWithCells()
    {
        if (_cells.Count < 1)
        {
            return;
        }

        //表示されている最後のセルに対応するリスト項目の次のリスト項目があり、
        //且つ、そのセルが_visibleRectの範囲内に入るようであれば、対応するセルを作成する
        TableViewCell<T> lastCell = _cells.Last.Value;
        int nextCellDataIndex = lastCell.DataIndex + 1;
        var nextCellTop = lastCell.Bottom + new Vector2(0.0f, -_spacingHeight);

        //1．セルがリスト総数を越えない事 
        //2．上から順に範囲内判定をしていくとすると、上端が範囲内にある事だけ確認すればよい。
        Debug.Log($"{nextCellTop.y + CachedScrollRect.content.localPosition.y} >= {_viewportRect.y}");
        while (nextCellDataIndex < _tableData.Count &&
            nextCellTop.y + CachedScrollRect.content.localPosition.y >= _viewportRect.y)
        {
            var cell = CreateCellForIndex(nextCellDataIndex);
            cell.Top = nextCellTop;

            lastCell = cell;
            nextCellDataIndex = lastCell.DataIndex + 1;
            nextCellTop = lastCell.Bottom + new Vector2(0.0f, -_spacingHeight);
        }
    }


    private Vector2 _prevScrollPos; //前回のスクロール位置を保持

    //イベント：スクロールビューがスクロールされた時に呼び出す
    public void OnScrollPosChanged(Vector2 scrollPos)
    {
        //_visibleRectを更新する。
        UpdateVisibleRect();
        //スクロールした方向に合わせて、セルを再利用と表示更新する。
        ReuseCells((scrollPos.y < _prevScrollPos.y) ? 1 : -1);

        _prevScrollPos = scrollPos;
    }

    /// <summary>
    ///セルを再利用して表示更新するメソッド
    /// </summary>
    /// <param name="scrollDirection">方向を符号付正数表す「上:正数１、下:負数-1」</param>
    private void ReuseCells(int scrollDirection)
    {
        if (_cells.Count <= 0)
        {
            return;
        }

        //スクロール方向は？
        if (scrollDirection > 0)
        {
            var content = CachedScrollRect.content;
            //上にスクロールしている場合
            //_visibleRectの範囲より上に移動したセルを、最下段に移動して内容更新する
            TableViewCell<T> firstCell = _cells.First.Value;
            while (firstCell.Bottom.y + ContentPosisionDelta > ViewportTop)
            {
                //先頭にあったセルのインデックスを最後尾にして内容を更新する。
                TableViewCell<T> lastCell = _cells.Last.Value;
                UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);
                firstCell.Top = lastCell.Bottom + new Vector2(0.0f, -_spacingHeight);

                _cells.AddLast(firstCell);
                _cells.RemoveFirst();
                // 次のセルへ引き継いで更新が必要であれば更新させる。
                firstCell = _cells.First.Value;
            }
            //_visibleRectの範囲内に空きがあればセルを作成する。
            FillVisibleRectWithCells();
        }
        else if (scrollDirection < 0)
        {
            //下にスクロールしている
            //_visibleRectの範囲より下に移動したセルを最上段に移動して内容更新する
            TableViewCell<T> lastCell = _cells.Last.Value;
            //TODO:　セルがViewport範囲内か？判定
            while (lastCell.Top.y + ContentPosisionDelta < ViewportBottom)
            {
                TableViewCell<T> firstCell = _cells.First.Value;
                UpdateCellForIndex(lastCell, firstCell.DataIndex - 1);
                lastCell.Bottom = firstCell.Top + new Vector2(0.0f, _spacingHeight);

                _cells.AddFirst(lastCell);
                _cells.RemoveLast();
                lastCell = _cells.Last.Value;
            }
        }
    }
}
