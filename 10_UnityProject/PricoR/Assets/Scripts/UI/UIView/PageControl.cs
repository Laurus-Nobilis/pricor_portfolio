using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PageingViewに対応させるページコントロールUI。
/// </summary>
public class PageControl : MonoBehaviour
{
    [SerializeField] Toggle _indicatorBase;    //ページコントロールのIndicator コピー元にする。
    private List<Toggle> _indicators = new List<Toggle>();    //Indicatorのリスト

    private void Awake()
    {
        // コピー元を消しておく。（MARK:勿体ないので余裕あれば何とかする）
        _indicatorBase.gameObject.SetActive(false);
    }

    /// <summary>
    /// ページ数を設定する
    /// </summary>
    /// <param name="pageNum">必要なページ数</param>
    public void SetNumberOfPage(int pageNum)
    {
        //既存のページ数と同数ならば何もしない。
        //指定された数が既存数より、多いか少ないか
        if (pageNum > _indicators.Count)
        {
            //不足分を追加する ex) cur:2 ,page:5
            for (int i = _indicators.Count; i < pageNum; i++)
            {
                Toggle indicator = Instantiate(_indicatorBase, _indicatorBase.transform.parent);
                indicator.gameObject.SetActive(true);
                indicator.isOn = false;
                indicator.transform.localScale = _indicatorBase.transform.localScale;
                _indicators.Add(indicator);
            }
        }
        else if(pageNum < _indicators.Count)
        {
            //余分を消す。Destroyが必要。 ex) cur 5, page:2
            for (int i = _indicators.Count - 1; i > pageNum; i--)
            {
                Destroy(_indicators[i].gameObject);
                _indicators.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Indicator表示を現在のページに合わせる
    /// </summary>
    /// <param name="index">現在のページを指す</param>
    public void SetCurrentPage(int index)
    {
        if (index < 0 || index >= _indicators.Count)
        {
            Debug.LogWarning("指定されたインデックスが範囲外です");
            return;
        }

        //トグルON (グループ使ってるので他は自動的にOFF)
        _indicators[index].isOn = true;
    }
}
