using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Assertions;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

// MEMO: prefabをアセットバンドルから読み込んでるために、変更時にその更新対応を忘れるとバグる。:注意

/// <summary>
/// 汎用ダイアログ。
/// static でどこからでも呼び出す。
/// その際prefabを読み込む。
/// </summary>
public class CommonDialogOkCancel : DialogBase
{
    [SerializeField] Button _okButton;
    [SerializeField] Button _cancelButton;
    [SerializeField] Text _text;
    
    GameObject _root;//閉じる際にDestroy対象。

    public static void Show()
    {
        Show(GetUICamera(), Director.Instance.transform, "テスト", () => { }, () => { });
    }

    public static void ShowOverlay()
    {
        ShowOverlay(Director.Instance.transform, "テスト", ()=>Debug.Log("OnClick!!"), null);
    }

    public static void Show(string message, Action cbkOk, Action cbkCancel)
    {
        Show(GetUICamera(), Director.Instance.transform, message, cbkOk, cbkCancel);
    }

    /// <summary>
    /// ScreenSpace - Camera　による表示。
    /// </summary>
    /// <param name="ui_camera"></param>
    /// <param name="parent"></param>
    /// <param name="cbkOk"></param>
    /// <param name="cbkCancel"></param>
    public static void Show(Camera ui_camera, Transform parent, string message, Action cbkOk, Action cbkCancel)
    {
        //await load prefab();
        Assert.IsNotNull(ui_camera);

        var go = InstantiatePrefab(parent);
        var canvas = go.GetComponentInParent<Canvas>();
        canvas.worldCamera = ui_camera;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.sortingLayerName = "UI";
        canvas.planeDistance = 1;

        var dlg = go.GetComponentInChildren<CommonDialogOkCancel>();

        Assert.IsNotNull(dlg);

        dlg.SetRoot(canvas);
        dlg.SetCallback(cbkOk, cbkCancel);
        dlg._text.text = message;
    }

    /// <summary>
    /// ScreenSpace - Overlay による表示で、シーン直下に生成追加する。
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="cbkOk"></param>
    /// <param name="cbkCancel"></param>
    /// <param name="sortOrder"></param>
    public static void ShowOverlay(string message, Action cbkOk, Action cbkCancel, int sortOrder = 100)
    {
        var go = InstantiatePrefab(null);
        var dlg = go.GetComponentInChildren<CommonDialogOkCancel>();

        Assert.IsNotNull(dlg);

        var canvas = go.GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortOrder;

        dlg.SetRoot(canvas);
        dlg.SetCallback(cbkOk, cbkCancel);
        dlg._text.text = message;
    }

    /// <summary>
    /// ScreenSpace - Overlay による表示。
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="cbkOk"></param>
    /// <param name="cbkCancel"></param>
    public static void ShowOverlay(Transform parent, string message, Action cbkOk, Action cbkCancel, int sortOrder = 100)
    {
        var go = InstantiatePrefab(parent);
        var canvas = go.GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortOrder;

        var dlg = go.GetComponentInChildren<CommonDialogOkCancel>();

        Assert.IsNotNull(dlg);

        dlg.SetRoot(canvas);
        dlg.SetCallback(cbkOk, cbkCancel);
        dlg._text.text = message;
    }

    static Camera GetUICamera()
    {
        var cam = GameObject.Find("Camera Front_UI").GetComponent<Camera>();
        Assert.IsNotNull(cam);
        return cam;
    }

    static GameObject InstantiatePrefab(Transform parent)
    {
        var prefab = CommonLib.AssetBundleHelper.LoadDialog("Assets/Prefab/CommonDialog_Canvas.prefab");
        GameObject go = null;
        if (parent == null)
        {
            //シーン直下に生成される。
            go = Instantiate(prefab);
        }
        else
        {
            go = Instantiate(prefab, parent);
        }
        Assert.IsNotNull(go);

        return go;
    }

    void SetRoot(Canvas canvas)
    {
        _root = canvas.gameObject;
    }

    void SetCallback(Action cbkOk, Action cbkCancel)
    {
        _okButton.OnClickAsObservable()
            .Subscribe(x =>
            {
                if(cbkOk!=null) cbkOk();
                Close();
            })
            .AddTo(gameObject);

        _cancelButton.OnClickAsObservable()
            .Subscribe(x =>
            {
                if(cbkCancel!=null) cbkCancel();
                Close();
            })
            .AddTo(gameObject);
    }

    void Close()
    {
        Debug.Log("On click close");
        Destroy(_root);
    }
}
