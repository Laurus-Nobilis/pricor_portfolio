using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Animator))]
public class UnitView : MonoBehaviour, IMenuView
{
    [SerializeField] Animator _anim;

    public void FadeIn()
    {
        _anim.Play("FadeIn");
    }

    public void FadeOut()
    {
        _anim.Play("FadeOut");
    }

    public void Init()
    {
    }
}
