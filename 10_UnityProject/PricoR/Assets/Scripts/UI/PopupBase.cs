using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// モーダルポップアップ・ベースクラス
/// </summary>
[RequireComponent(typeof(MaskableGraphic))]
public class PopupBase : MonoBehaviour
{
    [HideInInspector] public Canvas _rootCanvas = null; //親キャンバスをセットする。
    [HideInInspector] public bool IsModal { get; set; } = false;    //モーダルにするかどうか。
    private RectTransform _modalLayer = null;   //モーダルのレイアウト調整
    private MaskableGraphic _maskable = null;   //モーダルのGraphic Raycastを止める。

    public PopupBase(Canvas root)
    {
        _rootCanvas = root;
    }

    public void Show()
    {
        Assert.IsNotNull(_rootCanvas);

        if (IsModal)
        {
            // RectTransform Canvas全体をカバーして、中央にPopupを出す。
            var go = new GameObject();
            go.transform.SetParent(_rootCanvas.transform);
            _modalLayer = go.AddComponent<RectTransform>();
            _maskable = _modalLayer.gameObject.AddComponent<Image>();
            _maskable.color = new Color(0f, 0f, 0f, 0.5f);
            this.transform.SetParent(_modalLayer.transform);

            // 子のオブジェクトに親の影響を与えるため、親子関係を作ってからパラメータ設定する。
            _modalLayer.anchorMin = new Vector2(0,0);
            _modalLayer.anchorMax = new Vector2(1,1);
            _modalLayer.offsetMin = new Vector2(0, 0);
            _modalLayer.offsetMax = new Vector2(0, 0);
        }
        else
        {
            this.transform.SetParent(_rootCanvas.transform);
            this.transform.localPosition = new Vector3(0, 0, 0);
        }

        // EventTriggerの操作方法
        var trigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AsObservable()
            .Subscribe(x => Close())
            .AddTo(this);
        trigger.triggers.Add(entry);
    }

    public void Close()
    {
        if (_modalLayer != null)
        {
            Destroy(_modalLayer.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
