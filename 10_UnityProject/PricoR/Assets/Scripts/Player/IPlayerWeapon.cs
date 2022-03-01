using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeapon
{
    public void SetNormal();

    /// <summary>
    /// 攻撃開始フレームでの呼び出し
    /// </summary>
    public void AttackBegin();
    public void AttackPlaying();
    public void AttackEnd();
}
