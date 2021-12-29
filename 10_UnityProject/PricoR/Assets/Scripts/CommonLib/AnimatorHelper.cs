using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 試しに拡張メソッド。
/// </summary>
public static class AnimatorHelper
{
    //まずは現在のステートのnormalizedTimeを取得するアプローチです。
    //normalizedTimeはアニメーション開始時を0・再生後を1とするようにアニメーションの長さを正規化したものです。
    //アニメーションの再生状態はnormalizedTimeが0からスタートし、最終的には1になる訳です。

    //アニメーションがループしている場合とステートがAnimatorのグラフにより移動する場合はどうするか。
    //  ループは終わりが無いので除外するとして、
    //  Animetion Stateが遷移する場合はどうするか。

    public static bool IsStop(this Animator anim, int layer_index) 
    {
        return anim.GetCurrentAnimatorStateInfo(layer_index).normalizedTime >= 1;
    }
}
