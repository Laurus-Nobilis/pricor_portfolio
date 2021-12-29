using UnityEngine;
using UnityEngine.UI;

public class CharaEnhanceRect : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] SlideGuage _guage;
    [SerializeField] Image _image;

    public Image Image { get => _image; set => _image = value; }
    public SlideGuage Guage { get => _guage; set => _guage = value; }
    public Text Text { get => _text; set => _text = value; }
}
