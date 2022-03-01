using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵　ベースとして模索。
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] int _health = 10;

    private enum State
    {
        Move, //徘徊
        Search, //索敵
        Tracking, //移動
        Attack, //攻撃
        Damaged,//被弾
    }
    State _curState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDead())
        {
            return;
        }

        //StateMachine的な事やらねばな。
        switch(_curState)
        {
            //徘徊
            case State.Move:

            //索敵
            case State.Search:

            //移動
            case State.Tracking:

            //攻撃
            case State.Attack:

            //被弾
            case State.Damaged:

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当たったのがPlayerの攻撃か？
        if(other.CompareTag("PlayerAttack"))
        {
            // 引数が、Colliderである以上、攻撃の種類を判定するためにはGetComponent()せざるを得ないのだが、するか？？？
            //  そもそもTagが文字列比較というのも良く思えない。
            //  別案：Managerクラスで中央集権的にやるのがいいかも知れない。

            Debug.Log("HIT! OnDamaged!");
            _health -= 1;//(仮)
            //一度当たったら数秒間同じ物には当たらない事。
        }
    }

    bool IsDead() => _health <= 0;

    public void SetParam() 
    {
    }
}
