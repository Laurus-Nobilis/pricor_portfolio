using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自分のHealth状態を持つ。
/// ダメージを受け取り状態を更新する。
/// 状態の変更を伝える=>Actionか、Reactiveか、ここでは大差がない。
/// 伝える事＝ダメージを受けた事、Healthが０になった事。
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] float _maxHealth = 10.0f;

    public float CurrentHealth { get; set; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage, GameObject damage_src)
    {
        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        float trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f)
        {

        }

    }
}
