using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx.Triggers;
using UniRx;

[RequireComponent(typeof(CanvasGroup))]
public class NavigationViewController : ViewController
{
    //ビュー階層としてスタックする。
    private Stack<ViewController> _stackedView = new Stack<ViewController>();
    private ViewController _curView = null;

    [SerializeField] private Text _titleLabel;
    [SerializeField] private Button _backBtn;
    [SerializeField] private Text _backBtnLable;

    private void Awake()
    {
        //バックボタンはトップでは非表示
        _backBtn.gameObject.SetActive(false);

        //バックボタン押したコールバック (前のViewに戻る処理)
        _backBtn.OnClickAsObservable()
            .Subscribe(x=> { Pop(); })
            .AddTo(this);
    }

    // UIの操作を有効/無効と切り替える。
    private void EnableInteraction(bool enable)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = enable;
    }

    // 次の階層のビューへ遷移する処理を行うMethod
    public void Push(ViewController newView)
    {
        if(_curView == null)
        {
            //最初のビューはアニメーション不要です。
            newView.gameObject.SetActive(true);
            _curView = newView;
            return;
        }

        //アニメーション開始により遷移中は、UI操作を不可能にする。
        EnableInteraction(false);

        //今のView
        ViewController lastView = _curView;
        _stackedView.Push(lastView);
        //現在の位置からrectの幅だけ動かして画面外へ
        var lastViewPos = lastView.CachedRectTransform.anchoredPosition;
        lastViewPos.x -= this.CachedRectTransform.rect.width;
        var moveOutLastView = lastView.CachedRectTransform
            .DOLocalMoveX(lastViewPos.x, 0.3f)
            .SetEase(Ease.InOutQuint)
            .Play()
            .onComplete = ()=>
        {
            lastView.gameObject.SetActive(false);
        };

        //次のView
        newView.gameObject.SetActive(true);
        var newViewPos = newView.CachedRectTransform.anchoredPosition;
        newView.CachedRectTransform.anchoredPosition = new Vector2(this.CachedRectTransform.rect.width, newViewPos.y);
        newViewPos.x = 0.0f;
        var moveInNewView = newView.CachedRectTransform
            .DOLocalMoveX(newViewPos.x, 0.3f)
            .onComplete = () =>
            {
                EnableInteraction(true);
            };
        //moveInNewView.SetEase(Ease.InOutQuint).Play();

        //ヘッダー更新
        _curView = newView;
        _titleLabel.text = newView.Title;
        _backBtnLable.text = lastView.Title;
        _backBtn.gameObject.SetActive(true);
    }

    //前の階層へ戻す。
    public void Pop()
    {
        if (_stackedView.Count < 1)
        {
            return; //前のViewが無いので終了。
        }

        //アニメーション開始により遷移中は、UI操作を不可能にする。
        EnableInteraction(false);

        //現在のViewを画面外へ移動
        ViewController lastView = _curView;
        Vector2 lastViewPos = lastView.CachedRectTransform.anchoredPosition;
        lastViewPos.x = this.CachedRectTransform.rect.width;
        var moveOut = lastView.CachedRectTransform
            .DOLocalMoveX(lastViewPos.x, 0.3f)
            .SetEase(Ease.InOutCubic);
        moveOut.OnComplete(
            () => {
                lastView.gameObject.SetActive(false);
            });
        moveOut.Play();

        //スタックのViewを画面内へ移動
        ViewController popedView = _stackedView.Pop();
        popedView.gameObject.SetActive(true);
        var popedViewPos = popedView.CachedRectTransform.anchoredPosition;
        popedViewPos.x = 0f;
        popedView.CachedRectTransform
            .DOLocalMoveX(popedViewPos.x, 0.3f)
            .SetEase(Ease.InOutCubic)
            .OnComplete( () =>
            {
                EnableInteraction(true);
            });

        //ヘッダー状態更新
        _curView = popedView;
        _titleLabel.text = popedView.Title;
        //まだViewが積まれてるか？
        if (_stackedView.Count >= 1)
        {
            _backBtnLable.text = _stackedView.Peek().Title;
            _backBtn.gameObject.SetActive(true);
        }
        else
        {
            _backBtn.gameObject.SetActive(false);
        }
    }
}
