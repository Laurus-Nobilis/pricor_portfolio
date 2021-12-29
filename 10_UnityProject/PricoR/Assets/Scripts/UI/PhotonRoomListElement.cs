using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PhotonRoomListElement : MonoBehaviour
{
    [SerializeField] Button _btn;
    [SerializeField] Text _text;
    public string Name
    {
        get { return _text.text; }
        set { _text.text = value; }
    }

    public void SetCallback(UnityAction<string> action)
    {
        _btn.onClick.AddListener(()=> action(Name));
    }
}

