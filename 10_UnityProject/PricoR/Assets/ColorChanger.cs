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
    /// Animation event ����Ăяo���āA�����Ǝq�̐F��Tween�ŕω�������B
    /// </summary>
    /// <param name="event_data">float�A��Animator�̏����Q�Ƃ���B</param>
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
