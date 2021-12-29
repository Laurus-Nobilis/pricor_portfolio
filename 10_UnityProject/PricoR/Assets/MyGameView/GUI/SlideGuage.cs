using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System;

/// <summary>
/// 動かすとリビルド：RectTransformを動かしているからね。・・・
/// </summary>
public class SlideGuage : MonoBehaviour
{
    // 外部からこのプロパティに set して、ゲージを動かす。
    public ReactiveProperty<float> Percentage { get; set; } = new ReactiveProperty<float>(0f);

    [SerializeField] RectTransform _back;
    [SerializeField] RectTransform _fill;
    [SerializeField] float _paddingX = 0f;   //TODO：変更したらすぐEditorに反映できないかな？
    [SerializeField] float _paddingY = 0f;   //～
    float _max = 100f;
    float _min = 0.05f;//表示サイズ下限。最低この値から始める。


    void Start()
    {
        Assert.IsNotNull(_back);
        Assert.IsNotNull(_fill);
        CalcLayout();
        Percentage.AsObservable<float>()
            .Subscribe(x => {
                Debug.LogWarning("オブザーバブル : "+ x);
                SetValue(x);
                } )
            .AddTo(this);
    }

    void CalcLayout()
    {
        // paddingにもとづいて、fillのサイズ変更。
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
    /// 「０～１」の割合を入力する。
    /// </summary>
    /// <param name="percentage"></param>
    void SetValue(float percentage)
    {
        Debug.Log("SetValue,が呼ばれた。= "+percentage+"%");

        Assert.IsNotNull(_fill);
        percentage = percentage <= float.Epsilon ? 0 : Mathf.Max(percentage, _min);
        var size = _fill.sizeDelta;
        size.x = _max * percentage;
        _fill.sizeDelta = size;
    }
}
