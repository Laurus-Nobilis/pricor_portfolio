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
        Move, //œpœj
        Search, //õ“G
        Tracking, //ˆÚ“®
        Attack, //UŒ‚
        Damaged,//”í’e
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

        //StateMachine“I‚ÈŽ–‚â‚ç‚Ë‚Î‚ÈB
        switch(_curState)
        {
            //œpœj
            case State.Move:

            //õ“G
            case State.Search:

            //ˆÚ“®
            case State.Tracking:

            //UŒ‚
            case State.Attack:

            //”í’e
            case State.Damaged:

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // “–‚½‚Á‚½‚Ì‚ªPlayer‚ÌUŒ‚‚©H
        if(other.CompareTag("PlayerAttack"))
        {
            // ˆø”‚ªACollider‚Å‚ ‚éˆÈãAUŒ‚‚ÌŽí—Þ‚ð”»’è‚·‚é‚½‚ß‚É‚ÍGetComponent()‚¹‚´‚é‚ð“¾‚È‚¢‚Ì‚¾‚ªA‚·‚é‚©HHH
            //  ‚»‚à‚»‚àTag‚ª•¶Žš—ñ”äŠr‚Æ‚¢‚¤‚Ì‚à—Ç‚­Žv‚¦‚È‚¢B
            //  •ÊˆÄFManagerƒNƒ‰ƒX‚Å’†‰›WŒ “I‚É‚â‚é‚Ì‚ª‚¢‚¢‚©‚à’m‚ê‚È‚¢B


        }
    }

    bool IsDead() => _health <= 0;

    public void SetParam() 
    {
    }
}
