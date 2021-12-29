using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using DG.Tweening;
//�N���X����債�����́A���ԓI��肪����̂ł܂����삳���邱�Ƃ�D�悷��B

// Photon Loby Room Manager;
public class PhotonManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private const int POTON_SEND_RATE = 60;
    public enum Status
    {
        Connected,
        ConnectedFailed,
        Disconnected,
    };

    //GUI�n
    [SerializeField] Text _status;      //�󋵂�\��������B
    [SerializeField] InputDialog _inputDlg;//OK�Ńe�L�X�g���󂯎��B�A�\���l����K�v����B
    [SerializeField] Button _createBtn;     //���[���𐶐����Ă����ɓ���
    [SerializeField, Tooltip("Join Lobby")] Button _joinLobby;     //lobby�ɓ���B�i���̏ꍇ�͓���Ȃ��悤�ɂ��� ���� ���[���쐬�A�����_��join�j
    [SerializeField, Tooltip("Join Random")] Button _joinBtn;     //�����_���Ń��[���ɐڑ�����B
    [SerializeField] Button _cancelBtn;     //�L�����Z�����ĕ���B
    //Dlg
    [SerializeField] OkDialog _okDialogPrefab;    // Prefab
    OkDialog _okDlg;    // �C���X�^���X

    // ���[�����X�g
    [SerializeField] ScrollRect _listView;
    [SerializeField, Tooltip("���[�����X�g�G�������g")] PhotonRoomListElement _element;

    AppSettings _appSettings;

    /// <summary>
    /// �I�u�W�F�N�g�����A����C���^�[�t�F�[�X�����B
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }


    private void Start()
    {
        //PhotonNetwork.LoadLevel()�ɂ��V�[���؂�ւ��̓���
        PhotonNetwork.AutomaticallySyncScene = true;

        _createBtn.onClick.AddListener(() => _inputDlg.Show());
        _joinLobby.onClick.AddListener(() => JoinLobby());
        _joinBtn.onClick.AddListener(() => JoinRandom());
        _cancelBtn.onClick.AddListener(() => Cancel());
        _inputDlg.InputEndObservable
            .Subscribe(str => CreateRoom(str))
            .AddTo(this);
    }

    void Printf(string msg)
    {
        Debug.Log(msg);
    }

    /// <summary>
    /// �X�e�[�^�X�o�͂Ƃ��Ẵe�L�X�g���㏑��
    /// </summary>
    /// <param name="str"></param>
    private void SetStatus(string str)
    {
        _status.text = str;
    }
    /// <summary>
    /// �X�e�[�^�X�o�͂Ƃ��Ẵe�L�X�g��ǉ�
    /// </summary>
    /// <param name="str"></param>
    private void AddStatus(string str)
    {
        _status.text += str + "\n";
    }

    #region Callbacks------------------------------------------------


    public override void OnConnected()
    {
        Director.Instance.TouchGuard.SetEnable(false);

        base.OnConnected();

        //Check
        Debug.Log("Photon Connected");
        //_subject.OnNext(Status.Connected);
        _status.text = "OnConnected\n";
    }

    /// <summary>
    /// Called when the local user/client left a room, so the game's logic can clean up it's internal state.
    /// </summary>
    /// <remarks>
    /// When leaving a room, the LoadBalancingClient will disconnect the Game Server and connect to the Master Server.
    /// This wraps up multiple internal actions.
    ///
    /// Wait for the callback OnConnectedToMaster, before you use lobbies and join or create rooms.
    /// </remarks>
    public override void OnLeftRoom()
    {
        SetStatus("�ގ�");
    }

    /// <summary>
    /// Called after switching to a new MasterClient when the current one leaves.
    /// </summary>
    /// <remarks>
    /// This is not called when this client enters a room.
    /// The former MasterClient is still in the player list when this method get called.
    /// </remarks>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        AddStatus("Master client switched.");
    }

    /// <summary>
    /// Called when the server couldn't create a room (OpCreateRoom failed).
    /// </summary>
    /// <remarks>
    /// The most common cause to fail creating a room, is when a title relies on fixed room-names and the room already exists.
    /// </remarks>
    /// <param name="returnCode">Operation ReturnCode from the server.</param>
    /// <param name="message">Debug message for the error.</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        AddStatus("Create room failed.");
    }

    /// <summary>
    /// Called when a previous OpJoinRoom call failed on the server.
    /// </summary>
    /// <remarks>
    /// The most common causes are that a room is full or does not exist (due to someone else being faster or closing the room).
    /// </remarks>
    /// <param name="returnCode">Operation ReturnCode from the server.</param>
    /// <param name="message">Debug message for the error.</param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        AddStatus("Join room failed");
    }

    /// <summary>
    /// Called when this client created a room and entered it. OnJoinedRoom() will be called as well.
    /// </summary>
    /// <remarks>
    /// This callback is only called on the client which created a room (see OpCreateRoom).
    ///
    /// As any client might close (or drop connection) anytime, there is a chance that the
    /// creator of a room does not execute OnCreatedRoom.
    ///
    /// If you need specific room properties or a "start signal", implement OnMasterClientSwitched()
    /// and make each new MasterClient check the room's state.
    /// </remarks>
    public override void OnCreatedRoom()
    {
        AddStatus("Create room!");
    }

    /// <summary>
    /// Called on entering a lobby on the Master Server. The actual room-list updates will call OnRoomListUpdate.
    /// </summary>
    /// <remarks>
    /// While in the lobby, the roomlist is automatically updated in fixed intervals (which you can't modify in the public cloud).
    /// The room list gets available via OnRoomListUpdate.
    /// </remarks>
    public override void OnJoinedLobby()
    {
        AddStatus("Joined lobby.");
    }

    /// <summary>
    /// Called after leaving a lobby.
    /// </summary>
    /// <remarks>
    /// When you leave a lobby, [OpCreateRoom](@ref OpCreateRoom) and [OpJoinRandomRoom](@ref OpJoinRandomRoom)
    /// automatically refer to the default lobby.
    /// </remarks>
    public override void OnLeftLobby()
    {
        AddStatus("Left a lobby.");
    }

    /// <summary>
    /// Called after disconnecting from the Photon server. It could be a failure or intentional
    /// </summary>
    /// <remarks>
    /// The reason for this disconnect is provided as DisconnectCause.
    /// </remarks>
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log("Photon DisConnected");
        //_subject.OnNext(Status.Disconnected);
        AddStatus("Photon disconnected.");
    }

    /// <summary>
    /// Called when the Name Server provided a list of regions for your title.
    /// </summary>
    /// <remarks>Check the RegionHandler class description, to make use of the provided values.</remarks>
    /// <param name="regionHandler">The currently used RegionHandler.</param>
    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        AddStatus("Region list received.");
    }

    /// <summary>
    /// Called for any update of the room-listing while in a lobby (InLobby) on the Master Server.
    /// </summary>
    /// <remarks>
    /// Each item is a RoomInfo which might include custom properties (provided you defined those as lobby-listed when creating a room).
    /// Not all types of lobbies provide a listing of rooms to the client. Some are silent and specialized for server-side matchmaking.
    /// </remarks>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        AddStatus("Room list update.");

        ReloadRoomList(roomList);
    }

    /// <summary>
    /// Called when the LoadBalancingClient entered a room, no matter if this client created it or simply joined.
    /// </summary>
    /// <remarks>
    /// When this is called, you can access the existing players in Room.Players, their custom properties and Room.CustomProperties.
    ///
    /// In this callback, you could create player objects. For example in Unity, instantiate a prefab for the player.
    ///
    /// If you want a match to be started "actively", enable the user to signal "ready" (using OpRaiseEvent or a Custom Property).
    /// </remarks>
    public override void OnJoinedRoom()
    {
        AddStatus("Joined room.");

        ShowMemberWaitView();
    }

    /// <summary>
    /// Called when a remote player entered the room. This Player is already added to the playerlist.
    /// </summary>
    /// <remarks>
    /// If your game starts with a certain number of players, this callback can be useful to check the
    /// Room.playerCount and find out if you can start.
    /// </remarks>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddStatus("Player enterd room.");
    }

    /// <summary>
    /// Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
    /// </summary>
    /// <remarks>
    /// If another player leaves the room or if the server detects a lost connection, this callback will
    /// be used to notify your game logic.
    ///
    /// Depending on the room's setup, players may become inactive, which means they may return and retake
    /// their spot in the room. In such cases, the Player stays in the Room.Players dictionary.
    ///
    /// If the player is not just inactive, it gets removed from the Room.Players dictionary, before
    /// the callback is called.
    /// </remarks>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AddStatus("Player left a room.");
    }

    /// <summary>
    /// Called when a previous OpJoinRandom call failed on the server.
    /// </summary>
    /// <remarks>
    /// The most common causes are that a room is full or does not exist (due to someone else being faster or closing the room).
    ///
    /// When using multiple lobbies (via OpJoinLobby or a TypedLobby parameter), another lobby might have more/fitting rooms.<br/>
    /// </remarks>
    /// <param name="returnCode">Operation ReturnCode from the server.</param>
    /// <param name="message">Debug message for the error.</param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        AddStatus("Failed : Join randam.");
    }

    /// <summary>
    /// Called when the client is connected to the Master Server and ready for matchmaking and other tasks.
    /// </summary>
    /// <remarks>
    /// The list of available rooms won't become available unless you join a lobby via LoadBalancingClient.OpJoinLobby.
    /// You can join rooms and create them even without being in a lobby. The default lobby is used in that case.
    /// </remarks>
    public override void OnConnectedToMaster()
    {
        AddStatus("Connected to master.");
    }

    /// <summary>
    /// Called when a room's custom properties changed. The propertiesThatChanged contains all that was set via Room.SetCustomProperties.
    /// </summary>
    /// <remarks>
    /// Since v1.25 this method has one parameter: Hashtable propertiesThatChanged.<br/>
    /// Changing properties must be done by Room.SetCustomProperties, which causes this callback locally, too.
    /// </remarks>
    /// <param name="propertiesThatChanged"></param>
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        AddStatus("Room properties update.");

        UpdateRoomProperty(propertiesThatChanged.ToString());
    }

    /// <summary>
    /// Called when custom player-properties are changed. Player and the changed properties are passed as object[].
    /// </summary>
    /// <remarks>
    /// Changing properties must be done by Player.SetCustomProperties, which causes this callback locally, too.
    /// </remarks>
    ///
    /// <param name="targetPlayer">Contains Player that changed.</param>
    /// <param name="changedProps">Contains the properties that changed.</param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        AddStatus("Player properties update.");
    }

    /// <summary>
    /// Called when the server sent the response to a FindFriends request.
    /// </summary>
    /// <remarks>
    /// After calling OpFindFriends, the Master Server will cache the friend list and send updates to the friend
    /// list. The friends includes the name, userId, online state and the room (if any) for each requested user/friend.
    ///
    /// Use the friendList to update your UI and store it, if the UI should highlight changes.
    /// </remarks>
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        AddStatus("Friend list update.");
    }

    /// <summary>
    /// Called when your Custom Authentication service responds with additional data.
    /// </summary>
    /// <remarks>
    /// Custom Authentication services can include some custom data in their response.
    /// When present, that data is made available in this callback as Dictionary.
    /// While the keys of your data have to be strings, the values can be either string or a number (in Json).
    /// You need to make extra sure, that the value type is the one you expect. Numbers become (currently) int64.
    ///
    /// Example: void OnCustomAuthenticationResponse(Dictionary&lt;string, object&gt; data) { ... }
    /// </remarks>
    /// <see cref="https://doc.photonengine.com/en-us/realtime/current/reference/custom-authentication"/>
    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        AddStatus("Cstom authentication response" + data.ToString());
    }

    /// <summary>
    /// Called when the custom authentication failed. Followed by disconnect!
    /// </summary>
    /// <remarks>
    /// Custom Authentication can fail due to user-input, bad tokens/secrets.
    /// If authentication is successful, this method is not called. Implement OnJoinedLobby() or OnConnectedToMaster() (as usual).
    ///
    /// During development of a game, it might also fail due to wrong configuration on the server side.
    /// In those cases, logging the debugMessage is very important.
    ///
    /// Unless you setup a custom authentication service for your app (in the [Dashboard](https://dashboard.photonengine.com)),
    /// this won't be called!
    /// </remarks>
    /// <param name="debugMessage">Contains a debug message why authentication failed. This has to be fixed during development.</param>
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        AddStatus("Failed : Custom authentication. " + debugMessage);
    }

    //TODO: Check if this needs to be implemented
    // in: IOptionalInfoCallbacks
    public override void OnWebRpcResponse(OperationResponse response)
    {
        AddStatus("Web rpc response.");
    }

    //TODO: Check if this needs to be implemented
    // in: IOptionalInfoCallbacks
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        AddStatus("Lobby statistics update.");
    }

    /// <summary>
    /// Called when the client receives an event from the server indicating that an error happened there.
    /// </summary>
    /// <remarks>
    /// In most cases this could be either:
    /// 1. an error from webhooks plugin (if HasErrorInfo is enabled), read more here:
    /// https://doc.photonengine.com/en-us/realtime/current/gameplay/web-extensions/webhooks#options
    /// 2. an error sent from a custom server plugin via PluginHost.BroadcastErrorInfoEvent, see example here:
    /// https://doc.photonengine.com/en-us/server/current/plugins/manual#handling_http_response
    /// 3. an error sent from the server, for example, when the limit of cached events has been exceeded in the room
    /// (all clients will be disconnected and the room will be closed in this case)
    /// read more here: https://doc.photonengine.com/en-us/realtime/current/gameplay/cached-events#special_considerations
    /// </remarks>
    /// <param name="errorInfo">object containing information about the error</param>
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        AddStatus("Error info: " + errorInfo.Info);
    }


    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings(_appSettings);
        PhotonNetwork.SendRate = 60;

    }


    #endregion  Callbacks--------------------------------------------------


    /// <summary>
    /// �ŏ���Photon�ɐڑ�����B
    /// </summary>
    public void ConnectPhoton()
    {
        Debug.Log("ConnectPhoton");

        Director.Instance.TouchGuard.SetEnable(true);

        PhotonNetwork.ConnectUsingSettings();

        //��b�Ԃɑ��M����p�P�b�g�������܂��Ă�̂ł����ύX����B
        PhotonNetwork.SendRate = POTON_SEND_RATE;
        PhotonNetwork.SerializationRate = POTON_SEND_RATE;

        //�V�[���J�ڂ𓯊�����B
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public void CreateRoom(string room)
    {
        if (room == string.Empty)
        {
            Debug.LogWarning("Room���������B");
            return;
        }

        Debug.Log(string.Format("CreateRoom({0})", room));
        /*
				//room�I�v�V����
				RoomOptions roomOptions = new RoomOptions();
				roomOptions.MaxPlayers = 4; //���[���ő�l��
				roomOptions.IsOpen = true;  //��������
				roomOptions.IsVisible = true;   //���r�[���猩���镔���ɂ���
		*/
        //room�쐬
        PhotonNetwork.CreateRoom(room);
    }
    public void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public void JoinRandom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void JoinRoom(string roomName)
    {
        Debug.Log(string.Format("JoinRoom({0})", roomName));
        //room����
        PhotonNetwork.JoinRoom(roomName);
    }

    public void GotoBattle()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[PHOTON] IsMasterClient.GotoBattle");
            PhotonNetwork.LoadLevel("Battle");
        }
    }

    public void Search()
    {
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.
        }
    }

    public void Cancel()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        Close();
    }


    private RoomOptions GetRoomOption()
    {
        // ���[���̃J�X�^���v���p�e�B�̏����l
        var initialProps = new Hashtable();
        initialProps["DisplayName"] = $"{PhotonNetwork.NickName}�̕���";
        initialProps["Message"] = "�N�ł��Q��OK�I";

        // ���r�[�̃��[����񂩂�擾�ł���J�X�^���v���p�e�B�i�L�[�̔z��j
        var propsForLobby = new[] { "DisplayName", "Message" };

        // �쐬���郋�[���̃��[���ݒ���s��
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomProperties = initialProps;
        roomOptions.CustomRoomPropertiesForLobby = propsForLobby;

        return roomOptions;
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    private void Close()
    {
        Director.Instance.TouchGuard.SetEnable(true);
        transform.DOScaleY(0, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            Director.Instance.TouchGuard.SetEnable(false);
        });
    }
    private void ShowMemberWaitView()
    {
        _okDlg = GameObject.Instantiate(_okDialogPrefab
            , _status.transform.parent.transform.position
            , _status.transform.parent.transform.rotation
            , _status.transform.parent);
        _okDlg.SetButtonCallback(() =>
        {
            GotoBattle();
        }
        , () =>
        {
            LeaveRoom();
            Destroy(_okDlg.gameObject); //TODO:Destroy����Ȃ�B�v�[���ɓ���ēK�X�폜���B
        }
        , "Room,�ҋ@��");

    }
    private void UpdateRoomProperty(string msg)
    {
        Debug.LogWarning("RoomProperties���X�V���ꂽ : " + msg);
        _okDlg.SetText($"Room\n<RoomPropertiesUpdate>");
    }
    private void ReloadRoomList(List<RoomInfo> roomlist)
    {
        //TODO: ���݂̃��X�g��ێ����Ă����A�V�������X�g��������X�V����B�폜���ꂽ���ڂƒǉ����ꂽ���ڂ�����B

        List<RoomInfo> exist = new List<RoomInfo>();
        foreach (PhotonRoomListElement child in _listView.content.transform)
        {
            //LINQ FirstOrDefault . class �� default �́unull�v
            var f = roomlist.FirstOrDefault(o => o.Name == child.Name);
            if (f == null)
            {
                Destroy(child);//�n���ꂽ���X�g�Ɋ܂܂�Ă��Ȃ����������B
            }
            else
            {
                exist.Add(f);
            }
        }

        var newRoom = roomlist.Except(exist);
        foreach (var info in newRoom)
        {
            var elm = GameObject.Instantiate(_element, _listView.content);
            elm.SetCallback(x => JoinRoom(x));
            elm.Name = info.Name;
        }
    }

}
