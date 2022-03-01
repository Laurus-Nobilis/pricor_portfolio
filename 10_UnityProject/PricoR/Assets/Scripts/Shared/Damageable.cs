using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージを受け取れるオブジェクト。
/// 
/// 設計
/// 最初にダメージを受け取る窓口になる。
/// ダメージソースになるコライダの衝突時GetComponent<>などしてダメージを与えてもらう。
/// 与えられたダメージをHealthへ送る、Healthから本体へ伝播していく。
/// 
/// Colliderに付けてもらいたいが、
/// </summary>
[RequireComponent(typeof(Collider))]// < Colliderが数種類あるが可能か？
public class Damageable : MonoBehaviour
{
    [SerializeField] Health _health = null;

    private void Awake()
    {
        if(_health == null)
        {
            _health = GetComponentInParent<Health>();
        }
    }

    public void InflictDamage(float damage, GameObject damage_src)
    {
        Debug.Log("Damageable - damage " + damage.ToString());
        if (_health)
        {
            //ここで何かしらダメージ量に影響を付けるかもしれない

            _health.TakeDamage(damage, damage_src);
        }
    }
}
