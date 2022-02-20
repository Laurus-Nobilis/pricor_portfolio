using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Photon.Pun;

//カメラは自キャラを中心に回転移動する。
public class PlayerCamera_OLD : PlayerCameraBase
{
    [SerializeField] private Transform _tgt = null;
    [SerializeField] Vector3 _tgtDir = new Vector3(0, 0.5f, 0);
    [SerializeField] float _camDistance = 3f;//カメラとの距離。
    [SerializeField] private float _turnSpeed = 10.0f;
    Camera _cam = null;
    Quaternion _hRot;
    Quaternion _vRot;
    float _movX = 0;
    float _movY = 0;
    bool _moveCamera = true;

    //Property
    public Camera TrackCamera { get { return _cam; } }

    public override Quaternion HorizontalRot { get => _hRot; }

    private void Start()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        _cam = Camera.main;
        Assert.IsNotNull(_tgt);
        Assert.IsNotNull(_cam);

        _vRot = Quaternion.Euler(30, 0, 0);
        _hRot = Quaternion.identity;
        RotateCamera(0, 0);
        _cam.transform.position = CalcCamPosition();
        _cam.transform.LookAt(_tgt.position + _tgtDir);
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _moveCamera = !_moveCamera;
        }

        _movY = _movX = 0;
        if (_moveCamera)
        {
            //マウス移動量は既にDeltaTimeの影響下にある。
            _movX = Input.GetAxis("Mouse X");
            _movY = -Input.GetAxis("Mouse Y");
        }
        RotateCamera(_movX, _movY);

        _cam.transform.position = CalcCamPosition();
        _cam.transform.LookAt(_tgt.position + _tgtDir);
    }

    private void RotateCamera(float horizontal, float vertical)
    {
        // 角度制限を付ける。
        // 四元数からそのまま角度制限つける方法が分からなかったため、事前にオイラー角の方で制限する。
        float curVRot = (_vRot.eulerAngles.x > 180) ? _vRot.eulerAngles.x - 360 : _vRot.eulerAngles.x;
        float nextVertical = curVRot + vertical * _turnSpeed;
        nextVertical = Mathf.Clamp(nextVertical, -80, 80);

        _vRot = Quaternion.Euler(nextVertical, 0f, 0f);
        _hRot *= Quaternion.Euler(0f, horizontal * _turnSpeed, 0f);
        _cam.transform.rotation = _hRot * _vRot;
    }

    private Vector3 CalcCamPosition()
    {
        return transform.position - _cam.transform.rotation * Vector3.forward * _camDistance;
    }
}
