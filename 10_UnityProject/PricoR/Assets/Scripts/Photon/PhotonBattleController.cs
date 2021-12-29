using Photon.Pun;
using UnityEngine;

public class PhotonBattleController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private Transform _spawnPos;
    PlayerController _player;
    public void PlayerSpawn()
    {
        if (PhotonNetwork.IsConnected)
        {
            var go = PhotonNetwork.Instantiate(_characterPrefab.name, _spawnPos.position, _spawnPos.rotation, 0);
            _player = go?.GetComponent<PlayerController>();
        }
        else
        {
            var go = Instantiate(_characterPrefab, _spawnPos.position, _spawnPos.rotation);
            _player = go?.GetComponent<PlayerController>();
        }
    }

    public void DisableControl()
    {
        _player.enabled = false;
    }
    public void EnableControl()
    {
        _player.enabled = true;
    }
}