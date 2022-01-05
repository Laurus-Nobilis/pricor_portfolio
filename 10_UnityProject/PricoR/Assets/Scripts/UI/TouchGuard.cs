using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGuard : MonoBehaviour
{
    [SerializeField] RectTransform _guard;

    //TODO: �������A�Ăяo���񐔃J�E���g�Ȃǂ��邩�H
    /// <summary>
    /// �^�b�`�K�[�h����B�L��������؂�ւ���B
    /// Director�I�N���X����A�N�Z�X������B
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="transparent"></param>
    public void SetEnable(bool enable, bool transparent = false)
    {
        _guard.gameObject.SetActive(enable);
    }
}
