using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


/// <summary>
/// uGUI�̃A�j���[�V���������Ă݂����A���r���h������Ƃ������ŁA�{���͔��������B
/// Canvas�̕������AUIController���A�A�A
/// </summary>
/// 


public class ItemListManager : MonoBehaviour
{
    enum BtnType : int
    {
        Add,
        Remove,
        Buy,
    }
    bool IsFadeIn { get; set; }     //FadeIn���͑���s�Ƃ���B

    // view
    [SerializeField] private GameObject _content;      //Item������e
    [SerializeField] private ItemContent _itemPrefab;    //ItemCell
    [SerializeField] private ItemMaster _master;        //Master
    [SerializeField] private Button[] _buttons = new Button[3];//Add,Remove,Buy

    [SerializeField] private GameObject _inventoryListContent;  // �v���C���[�̃C���x���g���B
    [SerializeField] private InventoryContent _inventoryPrefab;

    [SerializeField] private SelectedTable _selectedTable;  //�I����������z�u����ꏊ�B
    private int _selectedItemIndex = -1;

    // �f�[�^��ǂݍ���ŁA�W�J�B
    // ���ԂɃA�j���[�V�������Đ����Ă��������B
    [SerializeField] private float _itemFadeInLagTime;   // ItemCell�����Ԃɏo���Ԋu�̎���

    private List<ItemContent> _itemListBuf = new List<ItemContent>();   // �\�����Ă��镨���Q�Ƃ��Ă����B


    // Start is called before the first frame update
    void Start()
    {
        IsFadeIn = true;
        StartCoroutine(FadeIn());

        Assert.AreEqual(_buttons.Length, 3, "�{�^���̐�������Ȃ�");
        _buttons[(int)BtnType.Add].onClick.AddListener(OnClickAdd);
        _buttons[(int)BtnType.Remove].onClick.AddListener(OnClickRemove);
        _buttons[(int)BtnType.Buy].onClick.AddListener(OnClickBuy);
    }

    void OnClickAdd()
    {
        var list = _master.items;
        if (list.Count == 0)
        {
            return;
        }

        var item = list[Random.Range(0, list.Count)];
        MakeItemButton(item);

        _buttons[(int)BtnType.Remove].interactable = true;
    }

    void OnClickRemove()
    {
        if (_itemListBuf.Count > 0)
        {
            RemoveItem(_itemListBuf.Count - 1);
        }
    }

    private IEnumerator FadeIn()
    {
        //�}�X�^�[����f�[�^�擾���āA���X�g�W�J�B
        //�J�n�AFadeIn���s�I����̃R�[���o�b�N�܂Őݒ�B
        //���X�g�W�J�A�j���[�V����
        foreach (var item in _master.items)
        {
            MakeItemButton(item);

            //if(��ʂ����܂����炠�Ƃ͖���)
            yield return new WaitForSeconds(_itemFadeInLagTime);
        }
    }

    void OnClickBuy()
    {
        if (_selectedItemIndex < 0)
        {
            Debug.LogAssertion("Index�l���s��");
            return;
        }

        ///  Inventory PlayerData�X�V����������폜�B
        //  ���N�G�X�g�@Player�w���۔���(�l�i�A�󂫘g)
        //  ���X�|���X�R�[���o�b�N�� �f�[�^�ƕ`����X�V�B

        //�@����@PlayerData�B
        var item = _itemListBuf[_selectedItemIndex];
        var content = Instantiate(_inventoryPrefab, Vector3.zero, Quaternion.identity, _inventoryListContent.transform);
        content.Init(item.ItemData);

        _buttons[(int)BtnType.Buy].interactable = false;
        _selectedTable.Clear();

        //���X�g����폜�B
        RemoveItem(_selectedItemIndex);
        _selectedItemIndex = -1;
    }

    /// <summary>
    /// ���X�g�{�^�������ꂽ���̃R�[���o�b�N�B
    /// </summary>
    void OnContentClickCallback(ItemContent item)
    {
        Assert.IsNotNull(_selectedTable);

        Debug.Log("onCallback" + item.Index + item.name);
        _selectedItemIndex = FindItemContentIndex(item);
        _selectedTable.SetItem(item.ItemData);

        _buttons[(int)BtnType.Buy].interactable = true;
    }

    /// <summary>
    /// �A�C�e���Z���p�̃v���t�@�u�𐶐����ă��X�g�֒ǉ��B
    /// </summary>
    /// <returns></returns>
    private void MakeItemButton(ItemData data)
    {
        var content = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _content.transform);
        content.Init(_itemListBuf.Count, data);
        content.SetCallback(OnContentClickCallback);

        _itemListBuf.Add(content);
    }

    private void RemoveItem(int index)
    {
        // �r���[�ƃo�b�t�@����폜����B
        Destroy(_itemListBuf[index].gameObject);
        _itemListBuf.RemoveAt(index);

        if (_itemListBuf.Count <= 0)
        {
            _buttons[(int)BtnType.Remove].interactable = false;
        }
    }

    private int FindItemContentIndex(ItemContent content)
    {
        for (int i = 0; i < _itemListBuf.Count; i++)
        {
            if(_itemListBuf[i] == content)
            {
                return i;
            }
        }

        return -1;
    }
}
