using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自分のHealthを監視して状態を更新する。
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] Health _health;

    [SerializeField] Image _fillImg;
    [SerializeField] Transform _canvasPivot;

    bool _hide = false;

    private void Start()
    {
        //_canvasPivot.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _fillImg.fillAmount = _health.CurrentHealth / _health.MaxHealth;

        // Health.CurrentがMaxの時は表示しない。一度表示させたらずっと出してても良い
        //if(_fillImg.fillAmount != 1)
        //{
        //    _canvasPivot.gameObject.SetActive(true);
        //}

        _canvasPivot.LookAt(Camera.main.transform);
    }
}
