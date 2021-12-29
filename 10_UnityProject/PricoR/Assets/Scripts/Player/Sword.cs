using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通の剣
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
        //Tagで攻撃判定させるため、攻撃中のTagに切り替える。
        _tagBuff = gameObject.tag;
        gameObject.tag = _ATTACK_TAG;
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    public void AttackPlaying()
    {
    }

    /// <summary>
    /// Trailが発生しないようにしつつ、
    /// 瞬間的に消えないように消える時間に合わせる。
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
