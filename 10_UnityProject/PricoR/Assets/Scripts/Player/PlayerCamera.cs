using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Photon.Pun;

/// <summary>
/// 
/// 先に Quaternion で行ったが、Vector3の方がシンプルで扱いやすい可能性を考えたので、作成。
/// </summary>
public class PlayerCamera : PlayerCameraBase
{
    [SerializeField] private Transform _forcusCenter = null; //カメラ中心にするもの
    [SerializeField] Vector3 _tgtForcusOffset = new Vector3(0, 0.5f, 0);  //カメラ中心からずのオフセット
    [SerializeField] float _camPosDistance = 3f;//カメラとの距離。
    [SerializeField] private float _turnSpeed = 10.0f;
    Camera _cam = null;
    float _camVerticalAngle = 0;
    float _camHorizontalAngle = 0;
    bool _moveCamera = true;

    public override Quaternion HorizontalRot
    {
        get
        {
            return Quaternion.Euler(0f, _cam.transform.eulerAngles.y, 0f);
        }
    }

    //Property
    public Camera TrackCamera { get { return _cam; } }

    private void Start()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        _cam = Camera.main;
        Assert.IsNotNull(_forcusCenter);
        Assert.IsNotNull(_cam);

        RotateCamera(0, 0);
        TranslateCamera();
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
            Cursor.lockState = _moveCamera ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (_moveCamera)
        {
            //マウス移動量は既にDeltaTimeの影響下にある。
            RotateCamera(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            TranslateCamera();
        }
    }

    private void TranslateCamera()
    {
        var pos = transform.position - _cam.transform.forward * _camPosDistance;
        //_cam.transform.position = Vector3.Lerp(_cam.transform.position, pos, 10f * Time.deltaTime);//< Lerp不要。操作性悪い。
        _cam.transform.position = pos;
    }

    /// <summary>
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    private void RotateCamera(float horizontal, float vertical)
    {
        _camHorizontalAngle += horizontal * _turnSpeed;
        _camVerticalAngle += vertical * _turnSpeed;
        _camVerticalAngle = Mathf.Clamp(_camVerticalAngle, -80f, 80f);
        _cam.transform.localEulerAngles = new Vector3(_camVerticalAngle, _camHorizontalAngle, 0.0f);
    }
}

