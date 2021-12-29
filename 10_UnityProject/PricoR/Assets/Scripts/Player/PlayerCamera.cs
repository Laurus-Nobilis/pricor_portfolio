using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Photon.Pun;
public class PlayerCamera : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _tgt = null;
    [SerializeField] Vector3 _tgtDir = new Vector3(0, 0.5f, 0);
    Camera _cam = null;
    [SerializeField] float _camDistance = 3f;//ƒJƒƒ‰‚Æ‚Ì‹——£B
    [SerializeField] private float _turnSpeed = 10.0f;
    Quaternion _hRot;
    Quaternion _vRot;

    //Property
    public Camera TrackCamera { get { return _cam; } }

    public Quaternion HorizontalRot { get => _hRot; }

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
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }
        var x = Input.GetAxis("Mouse X");
        var y = -Input.GetAxis("Mouse Y");

        RotateCamera(x,y);

        _cam.transform.position = CalcCamPosition();
        _cam.transform.LookAt(_tgt.position + _tgtDir);
    }

    private void RotateCamera(float move_x, float move_y)
    {
        _vRot *= Quaternion.Euler(move_y*_turnSpeed, 0f, 0f);
        _hRot *= Quaternion.Euler(0f, move_x * _turnSpeed, 0f);
        _cam.transform.rotation = _hRot * _vRot;
    }

    private Vector3 CalcCamPosition()
    {
        return transform.position - _cam.transform.rotation * Vector3.forward * _camDistance;
    }
}
