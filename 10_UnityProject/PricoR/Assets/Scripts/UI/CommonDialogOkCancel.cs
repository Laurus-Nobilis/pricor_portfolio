using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Assertions;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// 汎用ダイアログ。
/// static でどこからでも呼び出す。
/// その際prefabを読み込む。
/// </summary>
public class CommonDialogOkCancel : DialogBase
{
    [SerializeField] Button _okButton;
    [SerializeField] Button _cancelButton;
    
    GameObject _root;//閉じる際にDestroy対象。

    public static void Show()
    {
        Show(GetUICamera(), Director.Instance.transform, () => { }, () => { });
    }

    public static void ShowOverlay()
    {
        ShowOverlay(Director.Instance.transform, ()=>Debug.Log("OnClick!!"), null);
    }

    public static void Show(Action cbkOk, Action cbkCancel)
    {
        Show(GetUICamera(), Director.Instance.transform, cbkOk, cbkCancel);
    }

    /// <summary>
    /// ScreenSpace - Camera　による表示。
    /// </summary>
    /// <param name="ui_camera"></param>
    /// <param name="parent"></param>
    /// <param name="cbkOk"></param>
    /// <param name="cbkCancel"></param>
    public static void Show(Camera ui_camera, Transform parent, Action cbkOk, Action cbkCancel)
    {
        //await load prefab();
        Assert.IsNotNull(ui_camera);

        var go = InstantiatePrefab(parent);
        var dlg = go.GetComponentInChildren<CommonDialogOkCancel>();

        Assert.IsNotNull(dlg);

        var canvas = go.GetComponentInParent<Canvas>();
        //MARK: ScreenSpace - Cameraにする場合のカメラ設定を試した。＝＞ - Overlayを試しておく。
        canvas.worldCamera = ui_camera;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.sortingLayerName = "UI";
        canvas.planeDistance = 1;

        dlg.SetRoot(canvas);
        dlg.SetCallback(cbkOk, cbkCancel);
    }

    /// <summary>
    /// ScreenSpace - Overlay による表示。
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="cbkOk"></param>
    /// <param name="cbkCancel"></param>
    public static void ShowOverlay(Transform parent, Action cbkOk, Action cbkCancel, int sortOrder = 100)
    {
        var go = InstantiatePrefab(parent);
        var dlg = go.GetComponentInChildren<CommonDialogOkCancel>();

        Assert.IsNotNull(dlg);

        var canvas = go.GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortOrder;

        dlg.SetRoot(canvas);
        dlg.SetCallback(cbkOk, cbkCancel);
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
        var go = Instantiate(prefab, parent);
        //var go = Instantiate(prefab);//シーン直下に生成される。
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
