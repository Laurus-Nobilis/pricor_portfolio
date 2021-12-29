using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class OkDialog : DialogBase
{
    [SerializeField] Text _text;
    [SerializeField] Button _ok;
    [SerializeField] Button _cancel;


    public void SetButtonCallback(UnityAction ok, UnityAction cancel, string description)
    {
        _text.text = description;
        _ok.onClick.AddListener(ok);
        _cancel.onClick.AddListener(cancel);
    }

    public void SetText(string txt)
    {
        _text.text = txt;
    }

    #region ugui ドラッグ実験
    public Vector3 CalcCursorPoint_Perspective()
    {
        var mousePos = Input.mousePosition;
        //カメラがPerspectiveのときはz値の設定が必須となる。
        mousePos.z = 100f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    public Vector3 CalcCursorPoint_UICam()
    {
        var cams = GameObject.FindObjectsOfType<Camera>();
        Camera cam = null;
        foreach (var c in cams)
        {
            if(c.name == "UICamera")//名前 決め打ち。
            {
                cam = c;
                break;
            }
        }
        Assert.IsNotNull(cam);

        var mousePos = Input.mousePosition;
        return cam.ScreenToWorldPoint(mousePos);
    }

    public void BeginDrag()
    {
        Debug.Log($"％％％％％％％　Begin Drag : ");
    }
    public void Drag()
    {
        var cur = CalcCursorPoint_UICam();
        Debug.Log("％％％％％％％  Drag    : " + cur );
        cur.z = transform.position.z;
        transform.position = cur;//OrthoのUICameraにしたときは、画面とカーソルの座標が一致
    }
    public void EndDrag()
    {
        Debug.Log("％％％％％％％　End Drag");
    }
    #endregion
}
