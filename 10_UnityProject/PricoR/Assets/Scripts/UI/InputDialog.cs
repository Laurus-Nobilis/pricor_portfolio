using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

/// UniRxの利用。
/// 
/// <summary>
/// ■ダイアログ制御 on Prefab
/// ボタン１
/// テキスト入力１
/// </summary>
public class InputDialog : DialogBase 
{
    [SerializeField] Text _titleTxt;
    [SerializeField] InputField _input;
    [SerializeField] Button _btn;
    [SerializeField] Button _close;

    [SerializeField, Tooltip("開閉速度")] float _speed = 0.3f;

    Subject<string> _subject = new Subject<string>();
    public IObservable<string> InputEndObservable { get => _subject; }
    public Text TitleTxt { get => _titleTxt; set => _titleTxt = value; }


    // Start is called before the first frame update
    void Start()
    {
        _btn.OnClickAsObservable()
           .Subscribe(x => OnOK())
           .AddTo(this);
        _close.OnClickAsObservable()
            .Subscribe(x => OnCancel())
            .AddTo(this);
    }

    private void OnOK()
    {
        _subject.OnNext(_input.text);
        //_subject.OnCompleted();
        Close();
    }

    private void OnCancel()
    {
        _subject.OnNext(string.Empty);
        //_subject.OnCompleted();
        Close();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = new Vector3(1f, 0f, 1f);
        transform.DOScaleY(1f, _speed);
    }

    public void Close()
    {
        transform.DOScaleY(0f, _speed)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
