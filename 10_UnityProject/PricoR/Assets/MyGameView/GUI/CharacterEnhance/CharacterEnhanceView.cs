using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using System;
using UniRx;

/// <summary>
/// ポイントかアイテムを使ってキャラクターを強化。
/// </summary>
public class CharacterEnhanceView : MonoBehaviour
{
    //[SerializeField] CharacterEnhanceContent _charaContent;
    //[SerializeField] EnhanceMenu _enhanceMenu;    //右手側 強化項目選択View
    [SerializeField] LevelUpMenuRect _levelUpRect;
    [SerializeField] CharaEnhanceRect _charaEnhanceRect;
    [SerializeField] PotionUseRect _potionUseRect;
    [SerializeField] Button _toggle;
    [SerializeField] Button _saveButton;
    [SerializeField] Button _loadButton;
    int _selectCharaId;


    int _tmp_nextExp = 99999999;

    void Start()
    {
        //
        var userdata = UserData.Instance;
        //userdata.Items.Dump();
        List<ItemData> expItems = userdata.Items.ItemList.Where(x => x.type == ItemData.Type.ExpUp).ToList();
        _levelUpRect.InitList(expItems, OnClickExpItemUse);   //expItems.ToList());

        Debug.Assert(expItems.Count() > 0);

        //
        List<ItemData> potionItems = userdata.Items.ItemList.Where(x => x.type != ItemData.Type.ExpUp).ToList();
        _potionUseRect.InitList(potionItems, OnClickPotionUse);
 


        //
        InitCharaEnhansRect();

        //
        _saveButton.OnClickAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("SaveButton");
                var save = new LocalSaveData();
                save.SaveAsync<UserData>(UserData.Instance);
            })
            .AddTo(this);
        _loadButton.OnClickAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("LoadButton");
                var save = new LocalSaveData();
                var n = save.Load<UserData>();
                UserData.SetData(n);
                Refresh();
            })
            .AddTo(this);
        _toggle.OnClickAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("Toggle button");
                if (_levelUpRect.gameObject.activeSelf)
                {
                    _levelUpRect.gameObject.SetActive(false);
                    _potionUseRect.gameObject.SetActive(true);
                }
                else
                {
                     _levelUpRect.gameObject.SetActive(true);
                    _potionUseRect.gameObject.SetActive(false);
                }
            })
            .AddTo(this);
    }

    void Refresh()
    {
        InitCharaEnhansRect();
    }

    void InitCharaEnhansRect()
    {
        _charaEnhanceRect.Image.sprite = Resources.Load<Sprite>("kids_chuunibyou_girl");
        _charaEnhanceRect.Text.text = $"あと {_tmp_nextExp}";
        _charaEnhanceRect.Guage.Percentage.Value = UserData.Instance.Exp / _tmp_nextExp;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editorで変更があった場合（変な値にさせないとか、入力値に合わせて他を変化させるとか。
    /// ・オブジェクトの生成は出来ないとの事。
    /// </summary>
    void OnValidate()
    {
        Debug.Log("OnValidate of \"" + this.name + "\"");
    }
#endif

    void OnClickExpItemUse(ItemData item)
    {
        Debug.Log("Button down! : ExpItem");
        // 変更内容を保存して、Dirtyフラグを立てる。＝＞他の操作をしたらリクエストを投げる。
        // （何を何個使ったバッファにためておく。）

        //_charaEnhanceRect
        UserData.Instance.Exp += item.value;
        _charaEnhanceRect.Text.text = $"あと {_tmp_nextExp - UserData.Instance.Exp}";
        _charaEnhanceRect.Guage.Percentage.Value = (float)UserData.Instance.Exp / (float)_tmp_nextExp;
    }

    void OnClickPotionUse(ItemData item)
    {
        Debug.Log("Button down! : Use potion");

        _potionUseRect.RemoveItem(item);   
    }
}
