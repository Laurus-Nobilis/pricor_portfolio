using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGuard : MonoBehaviour
{
    [SerializeField] RectTransform _guard;

    //TODO: 透明化、呼び出し回数カウントなどするか？
    /// <summary>
    /// タッチガードする。有効無効を切り替える。
    /// Director的クラスからアクセスさせる。
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="transparent"></param>
    public void SetEnable(bool enable, bool transparent = false)
    {
        _guard.gameObject.SetActive(enable);
    }
}
