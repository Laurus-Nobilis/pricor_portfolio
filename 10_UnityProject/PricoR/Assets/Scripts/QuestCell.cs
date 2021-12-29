using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    public delegate void CbkOnClick(int quest);
    event CbkOnClick _callback;

    [SerializeField] Button _btn;
    public int QuestId { get; private set; }

    public void SetData(string title, int quest_id, CbkOnClick callback)
    {
        _btn.GetComponentInChildren<Text>().text = title;
        QuestId = quest_id;
        _callback = callback;
        if (_callback != null)
        {
            _btn.onClick.AddListener(() => _callback(QuestId));
        }
    }
}

