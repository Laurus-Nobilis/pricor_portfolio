using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeapon
{
    public void SetNormal();

    /// <summary>
    /// �U���J�n�t���[���ł̌Ăяo��
    /// </summary>
    public void AttackBegin();
    public void AttackPlaying();
    public void AttackEnd();
}
