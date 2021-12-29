using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
public class MainMenuManager : MonoBehaviour
{
    public enum ViewType
    {
        Home,
        Unit,
        Quest,
    }

    [SerializeField] Footer _footer;
    [SerializeField] HomeView _home;
    [SerializeField] UnitView _unit;
    [SerializeField] QuestView _quest;

    IMenuView _curView; //!!!: interface ��GameObject�������ĂȂ�����A�Q�Ƃł��Ȃ��B=>�Q�Ƃ���K�v���𖳂����΂悢��ȁB���ꂢ������abstruct�̕����ǂ���???

    private void Awake()
    {
        _home.gameObject.SetActive(false);
        _unit?.gameObject.SetActive(false);
        _quest.gameObject.SetActive(false);

    }
    private void Start()
    {

        //Footer�{�^���������ꂽ��View��؂�ւ��� Callback ��UniRx�ŁB
        //_footer.SetClickDelegates(GoHome, GoUnit, GoQuest);
        var o = _footer.Observable;
        o.Subscribe(x =>
        {
            Transit(x);
        }).AddTo(this);

        //������Ԃ���AFooter��Home�� FadeIn


        //Subject<Action> s = new UniRx.Subject<Action>();
        //var d = s.Subscribe();
        //s.OnNext(()=> { _home.gameObject.SetActive(true); });
        //s.OnCompleted();
        //d.Dispose();

        //Observable.EveryUpdate().Take(1)
        //    .Subscribe(_ =>
        //    {
        Debug.LogWarning("MainMenu Rx");
        _footer.gameObject.SetActive(true);
        _curView = _home;
        _home.Init();
        _home.gameObject.SetActive(true);

        //})
        //.AddTo(this);
    }

    private void Update()
    {

    }

    IEnumerator MenuLoop()
    {

        //Begin
        //FadeOut -> FadeIn
        //Idle
        //FadeIn -> FadeOut
        //End

        yield return null;
    }

    //RX�Ȃ�Subject���擾����Subscribe����΂����B�{�^���������ꂽ��OnNext(ViewType)���Ă���邩��B

    void Transit(ViewType nextView)
    {
        Debug.Log("Transit");
        switch (nextView)
        {
            case ViewType.Home:
                FadeStart(_home);
                Debug.Log("Home�{�^�����s�B");
                break;

            case ViewType.Unit:
                FadeStart(_unit);
                Debug.Log("Unit�{�^�����s�B");
                break;

            case ViewType.Quest:
                FadeStart(_quest);
                Debug.Log("Quest�{�^�����s�B");
                break;

            default:
                break;
        }
    }

    void FadeStart(IMenuView view)
    {
        if (_curView == view) { return; }
        //Director.Instance.TouchGuard.SetEnable(true);
        _curView?.FadeOut();
        view?.FadeIn();
        _curView = view;
    }

    void GoHome()
    {

    }

    void GoUnit()
    {

    }

    void GoQuest()
    {

    }
}
