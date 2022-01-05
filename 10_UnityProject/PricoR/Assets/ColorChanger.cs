using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColorAlphaRecursivery(float alpha)
    {
        foreach (var elm in GetComponentsInChildren<Image>())
        {
            var col = elm.color;
            col.a = alpha;
            elm.color = col;
        }
    }
    /// <summary>
    /// Animation event から呼び出して、自分と子の色をTweenで変化させる。
    /// </summary>
    /// <param name="event_data">float、とAnimatorの情報を参照する。</param>
    public void TweenChildrenColor(AnimationEvent event_data)
    {
        foreach (var elm in GetComponentsInChildren<Image>())
        {
            var col = elm.color;
            col.a = event_data.floatParameter;
            elm.DOColor(col, event_data.animatorClipInfo.clip.length);
        }
    }
}
