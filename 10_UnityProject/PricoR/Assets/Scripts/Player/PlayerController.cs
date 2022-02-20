using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Diagnostics;
using Photon.Pun;

// 必要なコンポーネントの列記
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    // Audio
    public AudioSource _audio;
    public AudioClip _attack;
    public AudioClip _walking;
    bool _isPlayWalkingAudio;
    // Camara
    [SerializeField] PlayerCamera_OLD _playerCam;
    float applySpeed = 0.2f;//振り向き速度。


    public float animSpeed = 1.5f;              // アニメーション再生速度設定
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                // このスイッチが入っていないとカーブは使われない
    public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

    // 以下キャラクターコントローラ用パラメタ
    // 前進速度
    public float forwardSpeed = 3.0f;
    // 後退速度
    public float backwardSpeed = 2.0f;
    // 旋回速度
    public float rotateSpeed = 2.0f;
    // ジャンプ威力
    public float jumpPower = 3.0f;
    // キャラクターコントローラ（カプセルコライダ）の参照
    private CapsuleCollider col;
    private Rigidbody rb;
    // キャラクターコントローラ（カプセルコライダ）の移動量
    private Vector3 velocity;
    // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
    private float orgColHight;
    private Vector3 orgVectColCenter;
    private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照


    // アニメーター各ステートへの参照
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");
    // !!!: 一旦ベタ書き
    static int attack1State = Animator.StringToHash("Base Layer.Attack1");
    static int attack2State = Animator.StringToHash("Base Layer.Attack2");
    static int attack3State = Animator.StringToHash("Base Layer.Attack3");


    // 自キャラのCapsuleにめり込み防止を行う。
    [SerializeField] CapsuleCollider _srcCapsule;
    List<string> _ignoreCollisionTags = new List<string>();

    // 初期化
    void Start()
    {
        _ignoreCollisionTags.Add("SelfWeapon");
        _ignoreCollisionTags.Add("PlayerAttack");

        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
        orgColHight = col.height;
        orgVectColCenter = col.center;

        var clickQ = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.E));
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Q))
            .Debug()
            .Buffer(2)
            .Scan(0, (acc, z) => acc + 1)
            .Debug()
            .Subscribe(e => Debug.Log("2回押された"))
            .AddTo(this);

        //weapon
        InitRightHandWeapon();
    }

    private void calcCapsuleStartEndPos(out Vector3 pos0, out Vector3 pos1)
    {
        pos0 = _srcCapsule.transform.position + _srcCapsule.transform.up * (_srcCapsule.height * 0.5f - _srcCapsule.radius);
        pos0 += _srcCapsule.center;
        pos1 = _srcCapsule.transform.position + _srcCapsule.transform.up * (_srcCapsule.height * 0.5f - _srcCapsule.radius) * -1;
        pos1 += _srcCapsule.center;
    }

    private void OnDrawGizmos()
    {
        var beforeCol = Gizmos.color;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_srcCapsule.bounds.min, _srcCapsule.bounds.max);

        Gizmos.DrawSphere(_srcCapsule.ClosestPoint(_srcCapsule.center), 0.1f);

        Gizmos.color = Color.red;
        calcCapsuleStartEndPos(out var pos0, out var pos1);
        //Gizmos.DrawSphere(pos0, _srcCapsule.radius);
        //Gizmos.DrawSphere(pos1, _srcCapsule.radius);

        //めり込みチェック
        Collider[] ret = new Collider[16];
        int count = Physics.OverlapCapsuleNonAlloc(pos0, pos1, _srcCapsule.radius, ret);//TODO:LayerMask 敵弾とかは無視してOKです。
        for (int i = 0; i < count; ++i)
        {
            var col = ret[i];
            if (col == _srcCapsule) { continue; }

            Physics.ComputePenetration(_srcCapsule, _srcCapsule.transform.position, _srcCapsule.transform.rotation
                , col, col.transform.position, col.transform.rotation, out var direction, out float distance);

            Gizmos.DrawLine(_srcCapsule.center, (direction.normalized * distance * 2));

            Debug.LogWarning("Oh my god.");
        }

        Gizmos.color = beforeCol;
    }

    private void BackPenetration()
    {
        calcCapsuleStartEndPos(out var pos0, out var pos1);
        //Physics.OverlapSphereNonAlloc()
        Collider[] ret = new Collider[16];
        int count = Physics.OverlapCapsuleNonAlloc(pos0, pos1, _srcCapsule.radius, ret);//TODO:LayerMask 敵弾とかは無視してOKです。
        Debug.LogWarning("count: " + count);
        if(count<=0)
        {
            return;
        }

        ret = ret.Where(col =>
        {
            
            if (col == _srcCapsule || col == null)
            {
                return false;
            }

            foreach (var str in _ignoreCollisionTags)
            {
                if (col.CompareTag(str))
                {
                    return false;
                }
            }
            return true;
        }).ToArray();   //.Linqで先に除外しておこうかと...
        for (int i = 0; i < ret.Count(); ++i)
        {
            var col = ret[i];
            //if (col == _srcCapsule
            //    || col.CompareTag("SelfWeapon")
            //    || col.CompareTag("PlayerAttack"))
            //{
            //    continue;
            //}

            Physics.ComputePenetration(_srcCapsule, _srcCapsule.transform.position, _srcCapsule.transform.rotation
                , col, col.transform.position, col.transform.rotation, out var direction, out float distance);

            var pos = transform.position + (direction * distance);
            this.transform.position = pos;
        }
    }

    //状態判定分離；
    private bool IsLocomotion() => anim.GetCurrentAnimatorStateInfo(0).nameHash == locoState;
    private bool IsJump() => (anim.GetCurrentAnimatorStateInfo(0).nameHash == jumpState);
    private bool IsIdle() => (anim.GetCurrentAnimatorStateInfo(0).nameHash == idleState);
    private bool IsRest() => (anim.GetCurrentAnimatorStateInfo(0).nameHash == restState);
    private bool IsAttack() => (
        anim.GetCurrentAnimatorStateInfo(0).nameHash == attack1State
        || anim.GetCurrentAnimatorStateInfo(0).nameHash == attack2State
        || anim.GetCurrentAnimatorStateInfo(0).nameHash == attack3State
        );
    //アクション
    private void Attack()
    {
        anim.Play("Attack1");
        if (HasWeapon())
        {
            _audio.Stop();
            _audio.clip = _attack;
            _audio.Play();

            //実行途中に連続で呼ばれないようにすること。
            StartCoroutine(WeaponAttack());
        }
    }


    private void Update()
    {
        WalkingAudio();
    }

    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
        float vertical = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
        bool attack = Input.GetMouseButtonDown(0);
        //anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
        //anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
        anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする


        if (Input.GetButtonDown("Jump"))
        {
            // スペースキーを入力したら

            //アニメーションのステートがLocomotionの最中のみジャンプできる
            if (currentBaseState.nameHash == locoState)
            {
                //ステート遷移中でなかったらジャンプできる
                if (!anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                }
            }
        }

        // 以下、キャラクターの移動処理
        velocity = new Vector3(horizontal, 0, vertical);        // 上下のキー入力からZ軸方向の移動量を取得
                                                // キャラクターのローカル空間での方向に変換
        velocity = velocity.sqrMagnitude < 1 ? velocity : velocity.normalized; // h:1,v:1だった場合に、斜移動だけ1を超えるのは良くないでしょ？
                                                                               // velocity = transform.TransformDirection(velocity);

        _isPlayWalkingAudio = false;
        //攻撃可能か？
        if (attack && !IsAttack() && !IsJump())
        {
            Attack();
        }
        else if (vertical > 0.1 || vertical < -0.1 || horizontal > 0.1 || horizontal < -0.1)
        {
            _isPlayWalkingAudio = true;
            velocity *= forwardSpeed;

            // 左右のキー入力でキャラクタをY軸で旋回させる
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(_playerCam.HorizontalRot * velocity),
                                                  applySpeed);
        }

        // 上下のキー入力でキャラクターを移動させる
        transform.localPosition += _playerCam.HorizontalRot * (velocity * Time.fixedDeltaTime);

        anim.SetFloat("Speed", velocity.sqrMagnitude);                          // Animator側で設定している"Speed"パラメタにvを渡す

        // 以下、Animatorの各ステート中での処理
        // Locomotion中
        // 現在のベースレイヤーがlocoStateの時
        if (currentBaseState.nameHash == locoState)
        {
            //カーブでコライダ調整をしている時は、念のためにリセットする
            if (useCurves)
            {
                resetCollider();
            }
        }
        // JUMP中の処理
        // 現在のベースレイヤーがjumpStateの時
        else if (currentBaseState.nameHash == jumpState)
        {
            if (!anim.IsInTransition(0))
            {

                // 以下、カーブ調整をする場合の処理
                if (useCurves)
                {
                    // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                    // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
                    // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
                    float jumpHeight = anim.GetFloat("JumpHeight");
                    float gravityControl = anim.GetFloat("GravityControl");
                    if (gravityControl > 0)
                        rb.useGravity = false;  //ジャンプ中の重力の影響を切る

                    // レイキャストをキャラクターのセンターから落とす
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                    RaycastHit hitInfo = new RaycastHit();
                    // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.distance > useCurvesHeight)
                        {
                            col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
                            float adjCenterY = orgVectColCenter.y + jumpHeight;
                            col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                        }
                        else
                        {
                            // 閾値よりも低い時には初期値に戻す（念のため）					
                            resetCollider();
                        }
                    }
                }
                // Jump bool値をリセットする（ループしないようにする）				
                anim.SetBool("Jump", false);
            }
        }
        // IDLE中の処理
        // 現在のベースレイヤーがidleStateの時
        else if (currentBaseState.nameHash == idleState)
        {
            //カーブでコライダ調整をしている時は、念のためにリセットする
            if (useCurves)
            {
                resetCollider();
            }
            // スペースキーを入力したらRest状態になる
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("Rest", true);
            }
        }
        // REST中の処理
        // 現在のベースレイヤーがrestStateの時
        else if (currentBaseState.nameHash == restState)
        {
            //cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
            // ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Rest", false);
            }
        }

        // めり込みを押し戻す。
        BackPenetration();
    }

    //void OnGUI()
    //{
    //    GUI.Box(new Rect(Screen.width - 260, 10, 250, 150), "Interaction");
    //    GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
    //    GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
    //    GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
    //    GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
    //    GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
    //    GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
    //}

    // キャラクターのコライダーサイズのリセット関数
    void resetCollider()
    {
        //Debug.Log("コライダーリセット");
        // コンポーネントのHeight、Centerの初期値を戻す
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }

    private void WalkingAudio()
    {
        if (_isPlayWalkingAudio)
        {
            if (_audio.clip != _walking)
            {
                _audio.clip = _walking;
                //_audio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);//複数の同じ音が鳴る場合、ピッチをずらして嫌な音を出さない。
                _audio.Play();
            }
        }
        else if(_audio.clip == _attack)
        {
            return;
        }
        else
        {
            _audio.Stop();
            _audio.clip = null;
        }
    }


    #region 武器系
    //!!!:  武器の取り扱いを適当にまとめておく。
    [SerializeField] Transform _rightHand;
    Sword _weapon;//TODO:GetComponent()する事を踏まえると、武器モノビヘイビアクラスが必要かもしれない。
    private void InitRightHandWeapon()
    {
        _weapon = _rightHand?.GetComponentInChildren<Sword>();
    }
    private bool HasWeapon()
    {
        return (_weapon != null);
    }
    private IEnumerator WeaponAttack()
    {
        if (_weapon == null) { yield break; }

        _weapon.AttackBegin();
        yield return null;

        while (IsAttack())
        {
            _weapon.AttackPlaying();
            yield return null;
        }

        _weapon.AttackEnd();
    }
    #endregion
}
