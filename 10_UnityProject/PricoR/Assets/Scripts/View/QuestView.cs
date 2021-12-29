using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class QuestView : MonoBehaviour, IMenuView
{
    [SerializeField] Animator _anim;
    [SerializeField] RectTransform _content;//���X�g�r���[�̓��ꕨ�B
    [SerializeField] QuestCell _srcCell;   //�Z��
    [SerializeField] RectTransform _questReady;

    Quest _questData;   // Quest�}�X�^

    public void FadeIn()
    {
        gameObject.SetActive(true);
        _anim.Play("FadeIn");
    }

    public void FadeOut()
    {
        _anim.Play("FadeOut");
        PlayEnd();
    }

    async void PlayEnd()
    {
        await UniTask.WaitUntil(() => _anim.IsStop(0));// GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        gameObject.SetActive(false);
    }

    public void Init()
    {
    }

    private void Start()
    {
        _questData = Resources.Load("Master/Quest") as Quest;
        foreach (var elm in _questData.dataArray)
        {
            var cell = GameObject.Instantiate(_srcCell, _content);
            cell?.SetData(elm.Name, elm.Id, x => OnSelectedToReady(x));
        }
    }

    private void OnSelectedToReady(int quest)
    {
        Director.Instance.TouchGuard.SetEnable(true);
        _questReady.gameObject.SetActive(true);
        _questReady.localScale = new Vector3(1f, 0f, 1f);
        _questReady.DOScaleY(1f, 0.3f)
            .OnComplete(() =>
            {
                Director.Instance.TouchGuard.SetEnable(false);
            });
    }
}
