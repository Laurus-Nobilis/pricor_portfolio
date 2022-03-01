using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Director : MonoBehaviour
{
    //  本体シングルトン
    static Director _instance = null;
    public static Director Instance {
        get
        {
            if (_instance)
            {
                return _instance;
            }
            Debug.LogError("MARK: 開放順を見直す。");
            _instance = new Director();
            return _instance;
        }
    }

    //タッチガード
    [SerializeField] TouchGuard _touchGuard;
    public TouchGuard TouchGuard { get => _touchGuard; }
    [SerializeField] Alert _alert;
    public Alert Alert { get => _alert; }

    // タップエフェクト
    [SerializeField] private TapFx _tapFx;

    /// ネットワーク
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
