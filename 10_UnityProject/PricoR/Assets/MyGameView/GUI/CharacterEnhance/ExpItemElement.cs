using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ExpUp�A�C�e���̃��X�g�r���[�ɓ������́B�@���G�Ȏ��͂����Ȃ��B
/// </summary>
public class ExpItemElement : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Text _number;
    [SerializeField] Text _name;
    [SerializeField] Button _button;
    public Image Image { get => _image; set => _image = value; }
    public Text Number { get => _number; set => _number = value; }
    public Text Name { get => _name; set => _name = value; }
    public Button Button { get => _button; set => _button = value; }
    public ItemData Item { get; set; }  //�Q�Ƃ��Z�b�g���Ă����B
    
}
