using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ʂ̌�
/// </summary>
public class Sword : MonoBehaviour, IPlayerWeapon
{
    [SerializeField] TrailRenderer[] _trailRenderers;

    const string _INVOKE_NORMAL = "SetNormal";
    const string _ATTACK_TAG = "PlayerAttack";
    string _tagBuff;

    private void OnEnable()
    {
        SetNormal();
    }

    private void SetTrailRanderer(bool enable)
    {
        CancelInvoke(_INVOKE_NORMAL);
        foreach (var t in _trailRenderers)
        {
            t.enabled = enable;
            t.emitting = enable;
        }
    }

    public void SetNormal()
    {
        SetTrailRanderer(false);
    }

    public void AttackBegin()
    {
        SetTrailRanderer(true);
        //Tag�ōU�����肳���邽�߁A�U������Tag�ɐ؂�ւ���B
        _tagBuff = gameObject.tag;
        gameObject.tag = _ATTACK_TAG;
    }

    /// <summary>
    /// ���t���[���X�V
    /// </summary>
    public void AttackPlaying()
    {
    }

    /// <summary>
    /// Trail���������Ȃ��悤�ɂ��A
    /// �u�ԓI�ɏ����Ȃ��悤�ɏ����鎞�Ԃɍ��킹��B
    /// </summary>
    public void AttackEnd()
    {
        float duration = 0;
        foreach(var t in _trailRenderers)
        {
            t.emitting = false;
            duration = Mathf.Max(duration, t.time);
        }

        gameObject.tag = _tagBuff;

        Debug.Log("TrailRenderer duation time. " + duration);
        Invoke(_INVOKE_NORMAL, duration);
    }
}
