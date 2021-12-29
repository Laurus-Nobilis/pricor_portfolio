using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Director : MonoBehaviour
{
    //  �{�̃V���O���g��
    static Director _instance = null;
    public static Director Instance {
        get
        {
            if (_instance)
            {
                return _instance;
            }
            Debug.LogError("TODO:�@�z��O�A�����������s���S. Awake��Ăяo�����͂�");
            _instance = new Director();
            return _instance;
        }
    }

    //�^�b�`�K�[�h
    [SerializeField] TouchGuard _touchGuard;
    public TouchGuard TouchGuard { get => _touchGuard; }
    [SerializeField] Alert _alert;
    public Alert Alert { get => _alert; }

    // �^�b�v�G�t�F�N�g
    [SerializeField] private TapFx _tapFx;

    /// �l�b�g���[�N
    NetworkManager _networkManager;
    public NetworkManager NetworkManager {
        get {
            return _networkManager;
        }
        private set => _networkManager = value; 
    }

    public void TapDisable() { _tapFx.enabled = false; }
    public void TapEnable() { _tapFx.enabled = true; }


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _instance.NetworkManager = new NetworkManager();
        Assert.IsNotNull(_instance.NetworkManager);

        DontDestroyOnLoad(_instance);
    }
}
