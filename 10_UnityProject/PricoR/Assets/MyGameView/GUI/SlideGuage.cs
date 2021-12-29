using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System;

/// <summary>
/// �������ƃ��r���h�FRectTransform�𓮂����Ă��邩��ˁB�E�E�E
/// </summary>
public class SlideGuage : MonoBehaviour
{
    // �O�����炱�̃v���p�e�B�� set ���āA�Q�[�W�𓮂����B
    public ReactiveProperty<float> Percentage { get; set; } = new ReactiveProperty<float>(0f);

    [SerializeField] RectTransform _back;
    [SerializeField] RectTransform _fill;
    [SerializeField] float _paddingX = 0f;   //TODO�F�ύX�����炷��Editor�ɔ��f�ł��Ȃ����ȁH
    [SerializeField] float _paddingY = 0f;   //�`
    float _max = 100f;
    float _min = 0.05f;//�\���T�C�Y�����B�ŒႱ�̒l����n�߂�B


    void Start()
    {
        Assert.IsNotNull(_back);
        Assert.IsNotNull(_fill);
        CalcLayout();
        Percentage.AsObservable<float>()
            .Subscribe(x => {
                Debug.LogWarning("�I�u�U�[�o�u�� : "+ x);
                SetValue(x);
                } )
            .AddTo(this);
    }

    void CalcLayout()
    {
        // padding�ɂ��ƂÂ��āAfill�̃T�C�Y�ύX�B
        var pos = _fill.localPosition;
        pos.x += _paddingX;
        _fill.localPosition = pos;
        
        var size = _fill.sizeDelta;
        size.x = _back.sizeDelta.x - _paddingX * 2;
        size.y = _back.sizeDelta.y - _paddingY * 2;
        _fill.sizeDelta = size;

        _max = size.x;
    }


    /// <summary>
    /// �u�O�`�P�v�̊�������͂���B
    /// </summary>
    /// <param name="percentage"></param>
    void SetValue(float percentage)
    {
        Debug.Log("SetValue,���Ă΂ꂽ�B= "+percentage+"%");

        Assert.IsNotNull(_fill);
        percentage = percentage <= float.Epsilon ? 0 : Mathf.Max(percentage, _min);
        var size = _fill.sizeDelta;
        size.x = _max * percentage;
        _fill.sizeDelta = size;
    }
}
