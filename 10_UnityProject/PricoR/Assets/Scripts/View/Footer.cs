using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;


[RequireComponent(typeof(Animator))]
public class Footer : MonoBehaviour, IMenuView
{
    [SerializeField] Button _home;
    [SerializeField] Button _unit;
    [SerializeField] Button _quest;
    [SerializeField] Animator _anim;
    List<Button> _btns = new List<Button>();//< めんどくさいかな。ボタン全部ON/OFFするための。

    // 使用者側がObserbableを通して通知を得る。
    Subject<MainMenuManager.ViewType> _subject = new Subject<MainMenuManager.ViewType>();
    public IObservable<MainMenuManager.ViewType> Observable { get => _subject.AsObservable(); }

    public void FadeIn()
    {
        _anim.Play("FadeIn");　// 全部パーツに書くのか？　めんどくさいな、なんかないのか？　一旦保留するぞ。
    }

    public void FadeOut()
    {
        _anim.Play("FadeOut");
    }

    public void Init()
    {
    }

    // Rx使わない場合はこんな感じ。
    //public void SetClickDelegates(UnityAction home, UnityAction unit, UnityAction quest)
    //{
    //    _home.onClick.AddListener(home);
    //    _unit.onClick.AddListener(unit);
    //    _quest.onClick.AddListener(quest);
    //}

    public void SetEnableButtons(bool enable)
    {
        foreach(var b in _btns)
        {
            b.interactable = enable;
        }
    }

    private void Awake()
    {
        this.gameObject.SetActive(false);

        _btns.Add(_home);
        _btns.Add(_unit);
        _btns.Add(_quest);

        _home.onClick.AddListener(() => _subject.OnNext(MainMenuManager.ViewType.Home));
        _unit.onClick.AddListener(() => _subject.OnNext(MainMenuManager.ViewType.Unit));
        _quest.onClick.AddListener(() => _subject.OnNext(MainMenuManager.ViewType.Quest));
    }
}
