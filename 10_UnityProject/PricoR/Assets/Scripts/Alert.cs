using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
public class Alert : MonoBehaviour
{
    [SerializeField] Canvas _canvas;    // DialogÇîzíuÇ∑ÇÈèäÅB
    public void Show(string msg)
    {
        Assert.IsNotNull(_canvas);

        var prefab = CommonLib.AssetBundleHelper.LoadDialog("Assets/Prefab/ErrorDialog.prefab")
                                                .GetComponent<ErrorDialog>();
        ErrorDialog dlg = Instantiate(prefab, _canvas.transform);
        dlg.gameObject.layer = _canvas.gameObject.layer;
        dlg.Show(msg);
    }
}
