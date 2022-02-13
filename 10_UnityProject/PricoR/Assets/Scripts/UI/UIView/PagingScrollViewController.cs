using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// ページビュー
/// ドラッグでページ切り替え。
/// ドラッグイベントを使うため[IBegin|IEnd]DragHandlerを継承
/// 
/// 移動方法について、AnimationCurveを使っているが、DOTweenなどでもよいだろう。
/// </summary>
public class PagingScrollViewController
    : ViewController, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] float _moveThreshold = 4;
    private bool _isAnimating = false;   //スクロールアニメーション実行中か？
    private Vector2 _destPos;   //移動先座標
    private Vector2 _initialPos;    //移動開始座標
    private AnimationCurve _animCurve;  //移動時のアニメーションCurve
    private int _prevPageIndex = 0;  //前ページのインデックス

    private ScrollRect _scrollRect;
    public ScrollRect CachedScrollRect  //外部からのアクセスさせていいか？ このプロパティにアクセス可能なクラスに制限を付けたいが可能か？
    {
        get
        {
            if (_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }
            return _scrollRect;
        }
    }

    private GridLayoutGroup _gridLayoutGroup;
    public GridLayoutGroup CachedGridLayoutGroup
    {
        get
        {
            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
            }
            return _gridLayoutGroup;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _isAnimating = false;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        var gridLayout = CachedGridLayoutGroup;

        //スクロールの動きを止める。
        CachedScrollRect.StopMovement();

        //1ページの幅を取得：GridLayoutGroupのCellSizeとspacingから
        float pageWidth = -(gridLayout.cellSize.x + gridLayout.spacing.x);

        //現在のスクロール位置から、フィットさせるページのインデックスを算出する。
        //  （ただし、要素Cellの幅がすべて同じ場合に限る)
        int pageIndex = Mathf.RoundToInt(CachedScrollRect.content.anchoredPosition.x / pageWidth);

        //ドラッグ速度判定
        if (pageIndex == _prevPageIndex && Mathf.Abs(eventData.delta.x) >= _moveThreshold)
        {
            //ドラッグが一定速度を超えていたら、その方向に1ページ進める
            CachedScrollRect.content.anchoredPosition += new Vector2(eventData.delta.x, 0.0f);
            //ドラッグ方向を、deltaの符号から判定し、現在のpageIndexを決める
            pageIndex += (int)Mathf.Sign(-eventData.delta.x);
        }

        //先頭や末尾のページの場合、それ以上先にスクロールしないようにする。
        pageIndex = Mathf.Clamp(pageIndex, 0, gridLayout.transform.childCount - 1);

        //現在のページのインデックスを保持する
        _prevPageIndex = pageIndex;

        //移動先座標を得る
        float destX = pageIndex * pageWidth;
        _destPos = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

        //開始時のスクロール位置を設定する。
        _initialPos = CachedScrollRect.content.anchoredPosition;

        //移動をスムーズにするアニメーションカーブ
        _animCurve = new AnimationCurve(
            new Keyframe(Time.time, 0.0f, 0.0f, 1.0f)
            , new Keyframe(Time.time + 0.3f, 1.0f, 0.0f, 0.0f));

        //フラグ：アニメーション中にする。
        _isAnimating = true;
    }

    private void LateUpdate()
    {
        if(!_isAnimating)
        {
            return;
        }

        //アニメーション終了か？（最後のキーフレームを過ぎたか？）
        if(Time.time >= _animCurve.keys[_animCurve.length - 1].time)
        {
            //アニメーションを終了させる
            CachedScrollRect.content.anchoredPosition = _destPos;
            _isAnimating = false;
            return;
        }

        //アニメーションカーブを使ってスクロールビューを移動させる。
        var newPos = _initialPos + (_destPos - _initialPos) * _animCurve.Evaluate(Time.time);
        CachedScrollRect.content.anchoredPosition = newPos;
    }
}
