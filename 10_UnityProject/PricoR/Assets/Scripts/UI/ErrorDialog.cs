using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// ■Errorダイアログ、Alertダイアログ。
/// 
/// use)
///  Prefabを生成して、Show(message) です。
/// </summary>
public class ErrorDialog : DialogBase
{
    [SerializeField] Text _message;
    [SerializeField] Button _btn;

    Subject<int> _subject = new Subject<int>();
    public IObservable<int> InputEndObservable { get => _subject; }
    public string MessageTxt { get => _message.text; set => _message.text = value; }

    // Start is called before the first frame update
    void Start()
    {
        _btn.OnClickAsObservable()
           .Subscribe(x => OnOK())
           .AddTo(this);
    }

    private void OnOK()
    {
        _subject.OnNext(0);
        Close();
    }

    /// <summary>
    /// メッセージを表示する。
    /// 時間設定されたら経過時間終了後削除
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="duration_time"></param>
    public void Show(string msg, int duration_time = 0)
    {
        gameObject.SetActive(true);
        MessageTxt = msg;
        if (duration_time > 0)
        {
            Invoke("Close", duration_time);
            _btn.gameObject.SetActive(false);
            _subject.Dispose();
        }
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}

