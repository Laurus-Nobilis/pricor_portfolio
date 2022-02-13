using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Animator))]
public class UnitView : MonoBehaviour, IMenuView
{
    [SerializeField] Animator _anim;

    public void FadeIn()
    {
        gameObject.SetActive(true);

        //TODO:DOTween
        _anim.Play("FadeIn");
    }

    public void FadeOut()
    {
        gameObject.SetActive(false);

        //TODO:DOTween
        _anim.Play("FadeOut");
    }

    public void Init()
    {
    }
}
