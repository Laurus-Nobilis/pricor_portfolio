using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ɋg�����\�b�h�B
/// </summary>
public static class AnimatorHelper
{
    //�܂��͌��݂̃X�e�[�g��normalizedTime���擾����A�v���[�`�ł��B
    //normalizedTime�̓A�j���[�V�����J�n����0�E�Đ����1�Ƃ���悤�ɃA�j���[�V�����̒����𐳋K���������̂ł��B
    //�A�j���[�V�����̍Đ���Ԃ�normalizedTime��0����X�^�[�g���A�ŏI�I�ɂ�1�ɂȂ��ł��B

    //�A�j���[�V���������[�v���Ă���ꍇ�ƃX�e�[�g��Animator�̃O���t�ɂ��ړ�����ꍇ�͂ǂ����邩�B
    //  ���[�v�͏I��肪�����̂ŏ��O����Ƃ��āA
    //  Animetion State���J�ڂ���ꍇ�͂ǂ����邩�B

    public static bool IsStop(this Animator anim, int layer_index) 
    {
        return anim.GetCurrentAnimatorStateInfo(layer_index).normalizedTime >= 1;
    }
}
