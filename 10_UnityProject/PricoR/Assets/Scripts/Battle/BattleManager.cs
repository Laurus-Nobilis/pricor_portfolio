using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


/// <summary>
/// シーン。ステージ制御：
/// ステージ選択後の流れ。
///     シーンマネージャーからシーンのロード。
/// ・対応するプレファブのロード。
/// ・シーンのAddtive
/// </summary>


public class BattleManager : MonoBehaviour
{
    [SerializeField] PhotonBattleController _gameMaker;
    public float _startDelay = 3f;
    public float _endDelay = 3f;
    private WaitForSeconds _startWait;     // Coroutin による遅延
    private WaitForSeconds _endWait;
    private string _nextScene = "MainMenu";


    [SerializeField] Animator _jingleAnim;

    private void Start()
    {
        // マウスカーソルを制御
        Cursor.lockState = CursorLockMode.Locked;
        
        _startWait = new WaitForSeconds(_startDelay);
        _endWait = new WaitForSeconds(_endDelay);
        _gameMaker.PlayerSpawn();
        StartCoroutine(GameLoop());

        //タップエフェクト無効
        Director.Instance.TapDisable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;

        //タップエフェクト有効
        Director.Instance.TapEnable();
    }

    private void LoadStage()
    {
        //
        //auto st = Director.Instance.GetInGameStatus();
        //
        // プレファブ読み込みなのか、シーンのAddtiveなのか
        //  事前にすべてのシーンを登録しておくのはめんどくさいぞ・・・それにアプリのビルドが必要になる。
        //　アセバンにシーンを追加して読み込めたりするのか？
        //　＝＞可能らしい　https://tsubakit1.hateblo.jp/entry/2016/08/23/233604　

    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        SceneManager.LoadScene(_nextScene);
    }

    private IEnumerator RoundStarting()
    {
        _gameMaker.DisableControl();//プレイヤーが操作できないように。
        yield return _startWait;
    }

    private IEnumerator RoundPlaying()
    {
        _gameMaker.EnableControl();
        while (!OnGoal())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        _gameMaker.DisableControl();
        _jingleAnim.Play("BattleResult");

        yield return _endWait;
    }


    private bool OnGoal()
    {
        //TODO: Battle終了をね。
        if (Input.GetKeyDown(KeyCode.C))
        {
            return true;
        }
        return false;
    }
}
