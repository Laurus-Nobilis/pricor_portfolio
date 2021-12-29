using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] int _health = 10;

    private enum State
    {
        Move, //�p�j
        Search, //���G
        Tracking, //�ړ�
        Attack, //�U��
        Damaged,//��e
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

        //StateMachine�I�Ȏ����˂΂ȁB
        switch(_curState)
        {
            //�p�j
            case State.Move:

            //���G
            case State.Search:

            //�ړ�
            case State.Tracking:

            //�U��
            case State.Attack:

            //��e
            case State.Damaged:

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������̂�Player�̍U�����H
        if(other.CompareTag("PlayerAttack"))
        {
            // �������ACollider�ł���ȏ�A�U���̎�ނ𔻒肷�邽�߂ɂ�GetComponent()������𓾂Ȃ��̂����A���邩�H�H�H
            //  ��������Tag���������r�Ƃ����̂��ǂ��v���Ȃ��B
            //  �ʈāFManager�N���X�Œ����W���I�ɂ��̂����������m��Ȃ��B


        }
    }

    bool IsDead() => _health <= 0;

    public void SetParam() 
    {
    }
}
