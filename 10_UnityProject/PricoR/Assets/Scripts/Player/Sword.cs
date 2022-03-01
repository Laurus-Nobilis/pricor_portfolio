using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通の剣
/// </summary>
public class Sword : MonoBehaviour, IPlayerWeapon
{
    [SerializeField] TrailRenderer[] _trailRenderers;
    [Tooltip("衝突判定の半径")]
    [SerializeField] private float Radius = 0.01f;
    float Damage { get; set; } = 5f;

    const string _INVOKE_NORMAL = "SetNormal";
    const string _ATTACK_TAG = "PlayerAttack";
    string _tagBuff;

    List<Collider> _ignoredColliders;   // 自分などとの接触を無視するため。
    bool _hitEnable = false;    //連続ヒットさせないフラグ

    private void Start()
    {
        var owner = GetComponentInParent<PlayerController>();
        Collider[] colliders = owner.gameObject.GetComponentsInChildren<Collider>();
        _ignoredColliders = new List<Collider>();
        _ignoredColliders.AddRange(colliders);
    }

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
        _hitEnable = true;
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    public void AttackPlaying()
    {
        //Hit detection -> hit to damage
        //　この剣がヒットしてるかを、Collisionイベントで取るか、SphereCastAllでとるかどうか.
        var raycast = new RaycastHit();
        raycast.distance = Mathf.Infinity;
        bool foundHit = false;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, Radius, transform.up, 2f);
        foreach (var hit in hits)
        {
            if (IsValidHit(hit) && hit.distance < raycast.distance)
            {
                foundHit = true;
                raycast = hit;
            }
        }
        if (foundHit)
        {
            Debug.Log("Attack Sphere cast all! " + raycast.collider.name.ToString());
            OnHit(raycast);
        }
    }

    /// <summary>
    /// Trailが発生しないようにしつつ、
    /// 瞬間的に消えないように消える時間に合わせる。
    /// </summary>
    public void AttackEnd()
    {
        _hitEnable = false;

        float duration = 0;
        foreach (var t in _trailRenderers)
        {
            t.emitting = false;
            duration = Mathf.Max(duration, t.time);
        }

        gameObject.tag = _tagBuff;

        Debug.Log("TrailRenderer duation time. " + duration);
        Invoke(_INVOKE_NORMAL, duration);
    }


    private bool IsValidHit(RaycastHit hit)
    {
        if (_ignoredColliders != null && _ignoredColliders.Contains(hit.collider))
        {
            return false;
        }

        if (hit.collider.GetComponent<Damageable>() == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ray"></param>
    private void OnHit(RaycastHit ray)
    {
        Debug.Log("Sword on hit!");
        ray.collider.GetComponent<Damageable>()?.InflictDamage(Damage, ray.collider.gameObject);
    }
}
