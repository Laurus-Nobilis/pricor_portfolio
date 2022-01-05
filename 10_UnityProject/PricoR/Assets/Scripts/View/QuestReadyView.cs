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
/// RectTransform の Quest系View
/// </summary>
public class QuestReadyView : MonoBehaviour
{
    [SerializeField] PhotonManager _photonMananger;
    [SerializeField, Tooltip("表示初期化")] Transform _photonView;
    [SerializeField, Tooltip("表示初期化")] Transform _photonInpuRoomDlg;
    [SerializeField] Button _goMulti;
    [SerializeField] Button _goSolo;
    [SerializeField] Button _back;
    ReactiveProperty<bool> _btnSwitcher = new ReactiveProperty<bool>();
    [SerializeField] RectTransform _panel;//この画面の親になるPanel
    

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
        //TODO:　このTweenアニメーションは汎用化してください。似た処理が分散してます。
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
        //TODO: パラメタ設定など
        await SceneManager.LoadSceneAsync("Battle");
    }

    private void Clear()
    {
        Debug.Log($"{gameObject.name}:View Clear");
    }
}
