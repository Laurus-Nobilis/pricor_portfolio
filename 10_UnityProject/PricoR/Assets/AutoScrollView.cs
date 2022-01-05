using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;


/// <summary>
/// 2ページを繰り返す ScrollView。(別途、複数要素を繰り返せるようにする）
/// </summary>
public class AutoScrollView : MonoBehaviour
{
    enum Direction : int
    {
        None,
        Left,
        Right,
    }

    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] RectTransform _content;
    [SerializeField] List<Image> _panels;
    int _left_index;//左に配置するパネルを指す添字
    int _right_index;//右に配置するパネルを指す添字
    float _viewWidth;

    float _prevPosRightEdge = 0;
    float _prevposLeftEdge = 0;

    Sequence _tweenSeq;
    void Start()
    {
        _left_index = 0;
        _right_index = _panels.Count - 1;
        _viewWidth = GetComponent<RectTransform>().sizeDelta.x;

        _prevPosRightEdge = -_viewWidth;
        _prevposLeftEdge = 0;

        //_tweenSeq = DOTween.Sequence();
        //_tweenSeq.Append(
        //    _content.DOLocalMoveX(-_viewWidth, 2)
        //    .SetEase(Ease.OutQuad)
        //    .SetDelay(2)
        //    );
        //_tweenSeq//.SetLoops(-1, LoopType.Incremental)//TODO:今回の実装は、SetLoops()と相性が悪く余計な切り替えが発生する。
        //    .Play()
        //    .SetLink(this.gameObject);//SetLinkでgameObjectと共に破棄
    }

    // Update is called once per frame
    void Update()
    {
        Wraparound();
    }

    /// <summary>
    /// Scroll方向を判定する
    /// </summary>
    /// <returns></returns>
    Direction MoveDirection()
    {
        var diff = _content.localPosition.x % _viewWidth;
        Debug.LogWarning("diff : " + diff);
        if (diff > 0)
        {
            return Direction.Right;
        }
        else if (diff < 0)
        {
            return Direction.Left;
        }
        return Direction.None;
    }


    /// <summary>
    /// パネルを周り込ませる。
    /// 
    /// 当初の問題、当初は、ScrollRect以下のcontentオブジェクトを　ドラッグしている間はlocalPosionがresetされてないこと。
    /// 
    /// </summary>
    /// <param name="dir"></param>
    void Wraparound()
    {
        var offset = _content.offsetMax;
        //_panels[_right_index].rectTransform.offsetMaxK
        if (offset.x < _prevPosRightEdge - float.Epsilon)
        {
            _prevPosRightEdge -= _viewWidth;
            _prevposLeftEdge -= _viewWidth;


            Debug.Log($"_content.offsetMax :: if({offset.x:F5} < {_prevPosRightEdge - float.Epsilon:F5})");
            var diff = new Vector3(-_viewWidth, 0f);
            var cur = _panels[_left_index].rectTransform.localPosition;
            _panels[_left_index].rectTransform.localPosition = _panels[_right_index].rectTransform.localPosition - diff;
            _panels[_right_index].rectTransform.localPosition = cur - diff;
        }
        else if (offset.x > _prevposLeftEdge + float.Epsilon)
        {
            Debug.Log("LeftEdge！！！！！！！");
            _prevPosRightEdge += _viewWidth;
            _prevposLeftEdge += _viewWidth;

            var diff = new Vector3(_viewWidth, 0f);
            var cur = _panels[_left_index].rectTransform.localPosition;
            _panels[_left_index].rectTransform.localPosition = _panels[_right_index].rectTransform.localPosition - diff;
            _panels[_right_index].rectTransform.localPosition = cur - diff;
        }
    }
}
