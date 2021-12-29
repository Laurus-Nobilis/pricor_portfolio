using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectedTable : MonoBehaviour
{
    public Image _icon;
    public Text _name;
    public Text _cost;

    // Start is called before the first frame update
    void Start()
    {
        _icon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(ItemData item)
    {
        _name.text = item.name;
        _cost.text = item.cost.ToString();
        _icon.sprite = Resources.Load<Sprite>($"RPGIcon/{item.asset_id}");
        _icon.enabled = true;
    }

    public void Clear()
    {
        _cost.text = _name.text = string.Empty;
        _icon.enabled = false;
    }
}
