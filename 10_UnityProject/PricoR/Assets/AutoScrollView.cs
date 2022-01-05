using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;


/// <summary>
/// 2�y�[�W���J��Ԃ� ScrollView�B(�ʓr�A�����v�f���J��Ԃ���悤�ɂ���j
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
    int _left_index;//���ɔz�u����p�l�����w���Y��
    int _right_index;//�E�ɔz�u����p�l�����w���Y��
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
        //_tweenSeq//.SetLoops(-1, LoopType.Incremental)//TODO:����̎����́ASetLoops()�Ƒ����������]�v�Ȑ؂�ւ�����������B
        //    .Play()
        //    .SetLink(this.gameObject);//SetLink��gameObject�Ƌ��ɔj��
    }

    // Update is called once per frame
    void Update()
    {
        Wraparound();
    }

    /// <summary>
    /// Scroll�����𔻒肷��
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
    /// �p�l�������荞�܂���B
    /// 
    /// �����̖��A�����́AScrollRect�ȉ���content�I�u�W�F�N�g���@�h���b�O���Ă���Ԃ�localPosion��reset����ĂȂ����ƁB
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
            Debug.Log("LeftEdge�I�I�I�I�I�I�I");
            _prevPosRightEdge += _viewWidth;
            _prevposLeftEdge += _viewWidth;

            var diff = new Vector3(_viewWidth, 0f);
            var cur = _panels[_left_index].rectTransform.localPosition;
            _panels[_left_index].rectTransform.localPosition = _panels[_right_index].rectTransform.localPosition - diff;
            _panels[_right_index].rectTransform.localPosition = cur - diff;
        }
    }
}
