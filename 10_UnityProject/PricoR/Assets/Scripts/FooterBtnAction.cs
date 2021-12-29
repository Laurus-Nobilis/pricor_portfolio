using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterBtnAction : MonoBehaviour
{
    [SerializeField] Animator _anim;

    public void OnClick()
    {
        Debug.Log("OnClick -- Footer");
        _anim.SetTrigger("Click");
    }
}
