using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UIView(Canvas向け)のベースクラス
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ViewController : MonoBehaviour
{
    //ViewのRectTransform
    private RectTransform _cachedRectTransform;
    public RectTransform CachedRectTransform
    {
        get
        {
            if (_cachedRectTransform == null)
            {
                _cachedRectTransform = GetComponent<RectTransform>();
            }
            return _cachedRectTransform;
        }
    }

    //Viewのタイトル
    public virtual string Title { get { return ""; } set { } }
}
