using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ���[�_���|�b�v�A�b�v�E�x�[�X�N���X
/// </summary>
[RequireComponent(typeof(MaskableGraphic))]
public class PopupBase : MonoBehaviour
{
    [HideInInspector] public Canvas _rootCanvas = null; //�e�L�����o�X���Z�b�g����B
    [HideInInspector] public bool IsModal { get; set; } = false;    //���[�_���ɂ��邩�ǂ����B
    private RectTransform _modalLayer = null;   //���[�_���̃��C�A�E�g����
    private MaskableGraphic _maskable = null;   //���[�_����Graphic Raycast���~�߂�B

    public PopupBase(Canvas root)
    {
        _rootCanvas = root;
    }

    public void Show()
    {
        Assert.IsNotNull(_rootCanvas);

        if (IsModal)
        {
            // RectTransform Canvas�S�̂��J�o�[���āA������Popup���o���B
            var go = new GameObject();
            go.transform.SetParent(_rootCanvas.transform);
            _modalLayer = go.AddComponent<RectTransform>();
            _maskable = _modalLayer.gameObject.AddComponent<Image>();
            _maskable.color = new Color(0f, 0f, 0f, 0.5f);
            this.transform.SetParent(_modalLayer.transform);

            // �q�̃I�u�W�F�N�g�ɐe�̉e����^���邽�߁A�e�q�֌W������Ă���p�����[�^�ݒ肷��B
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

        // EventTrigger�̑�����@
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
