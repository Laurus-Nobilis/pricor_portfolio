using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UniRx;
using DG.Tweening;
using UnityEngine.Assertions;

/// <summary>
/// RectTransform �� Quest�nView
/// </summary>
public class QuestReadyView : MonoBehaviour
{
    [SerializeField] PhotonManager _photonMananger;
    [SerializeField, Tooltip("�\��������")] Transform _photonView;
    [SerializeField, Tooltip("�\��������")] Transform _photonInpuRoomDlg;
    [SerializeField] Button _goMulti;
    [SerializeField] Button _goSolo;
    [SerializeField] Button _back;
    ReactiveProperty<bool> _btnSwitcher = new ReactiveProperty<bool>();
    [SerializeField] RectTransform _panel;//���̉�ʂ̐e�ɂȂ�Panel
    

    public void FadeIn()
    {
        Director.Instance.TouchGuard.SetEnable(true);
        gameObject.SetActive(true);
        _panel.localScale = new Vector3(1f, 0f, 1f);
        _panel.DOScaleY(1f, 0.23f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Director.Instance.TouchGuard.SetEnable(false);
            });
    }
    public void FadeOut()
    {
        Director.Instance.TouchGuard.SetEnable(true);
        _panel.localScale = new Vector3(1f, 1f, 1f);
        _panel.DOScaleY(0f, 0.23f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                Director.Instance.TouchGuard.SetEnable(false);
            });
    }

    private void Awake()
    {
    }

    private void Start()
    {
        _btnSwitcher.AsObservable()
            .Subscribe(x =>
            {
                _goMulti.interactable = x;
                _goSolo.interactable = x;
            })
            .AddTo(this);
        _btnSwitcher.Value = !_photonView.gameObject.activeSelf;

        _goMulti.OnClickAsObservable()
            .Subscribe(x =>
            {
                OnMultiReady();
            })
            .AddTo(this);

        _goSolo.OnClickAsObservable()
            .Subscribe(x => GotoSolo())
            .AddTo(this);

        _back.OnClickAsObservable()
            .Subscribe(x => GotoBack())
            .AddTo(this);
    }

    public void OnMultiReady()
    {
        //TODO:�@����Tween�A�j���[�V�����͔ėp�����Ă��������B�������������U���Ă܂��B
        Director.Instance.TouchGuard.SetEnable(true);
        _photonView.gameObject.SetActive(true);
        _photonView.localScale = new Vector3(1f, 0f, 1f);
        _photonView.DOScaleY(1f, 0.3f)
            .OnComplete(() =>
            {
                Director.Instance.TouchGuard.SetEnable(false);
            });
        _photonMananger.ConnectPhoton();
    }

    public void GotoSolo()
    {
        LoadBattle();
    }

    public void GotoBack()
    {
        FadeOut();
    }

    async void LoadBattle()
    {
        //TODO: �p�����^�ݒ�Ȃ�
        await SceneManager.LoadSceneAsync("Battle");
    }

    private void Clear()
    {
        Debug.Log($"{gameObject.name}:View Clear");
    }
}
