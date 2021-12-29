using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] PhotonBattleController _gameMaker;
    public float _startDelay = 3f;
    public float _endDelay = 3f;
    private WaitForSeconds _startWait;     // Coroutin �ɂ��x��
    private WaitForSeconds _endWait;
    private string _nextScene = "MainMenu";


    [SerializeField] Animator _jingleAnim;

    private void Start()
    {
        // �}�E�X�J�[�\���𐧌�
        Cursor.lockState = CursorLockMode.Locked;

        _startWait = new WaitForSeconds(_startDelay);
        _endWait = new WaitForSeconds(_endDelay);
        _gameMaker.PlayerSpawn();
        StartCoroutine(GameLoop());

        //�^�b�v�G�t�F�N�g����
        Director.Instance.TapDisable();
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;

        //�^�b�v�G�t�F�N�g�L��
        Director.Instance.TapEnable();
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
        _gameMaker.DisableControl();//�v���C���[������ł��Ȃ��悤�ɁB
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
        //TODO: Battle�I�����ˁB
        if (Input.GetKeyDown(KeyCode.C))
        {
            return true;
        }
        return false;
    }
}
