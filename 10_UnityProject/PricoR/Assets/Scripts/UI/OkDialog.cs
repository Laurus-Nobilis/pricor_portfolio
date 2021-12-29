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

    #region ugui �h���b�O����
    public Vector3 CalcCursorPoint_Perspective()
    {
        var mousePos = Input.mousePosition;
        //�J������Perspective�̂Ƃ���z�l�̐ݒ肪�K�{�ƂȂ�B
        mousePos.z = 100f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    public Vector3 CalcCursorPoint_UICam()
    {
        var cams = GameObject.FindObjectsOfType<Camera>();
        Camera cam = null;
        foreach (var c in cams)
        {
            if(c.name == "UICamera")//���O ���ߑł��B
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
        Debug.Log($"���������������@Begin Drag : ");
    }
    public void Drag()
    {
        var cur = CalcCursorPoint_UICam();
        Debug.Log("��������������  Drag    : " + cur );
        cur.z = transform.position.z;
        transform.position = cur;//Ortho��UICamera�ɂ����Ƃ��́A��ʂƃJ�[�\���̍��W����v
    }
    public void EndDrag()
    {
        Debug.Log("���������������@End Drag");
    }
    #endregion
}
