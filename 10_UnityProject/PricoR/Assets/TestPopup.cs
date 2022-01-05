using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestPopup : MonoBehaviour
{
    public PopupBase _popupPrefab;
    private PopupBase _popup;
    public void OnClick()
    {
        _popup = Instantiate<PopupBase>(_popupPrefab);
        _popup._rootCanvas = FindObjectOfType<Canvas>();
        _popup.IsModal = true;
        _popup.Show();
    }
}
