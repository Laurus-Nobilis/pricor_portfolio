using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/*やっぱり、BaseClass作った方がよいかなと。Fadeはパターン化されてるし。*/

public class HomeView : MonoBehaviour , IMenuView
{
    [SerializeField] Text _nameTxt;
    [SerializeField] Text _rankTxt;
    [SerializeField] Text _expTxt;
    [SerializeField] Slider _expSlider;//NextLevel

    [SerializeField] Animator _anim;

    public void Init()
    {
        var user = Models.Instance.GetUser();
        if (user == null) { return; }

        _nameTxt.text = user.Name;
        _rankTxt.text = user.Rank.ToString();
        _expTxt.text = user.Exp.ToString();
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        _anim.Play("FadeIn");
    }

    public void FadeOut()
    {
        _anim.Play("FadeOut");
        PlayEnd();
    }
    async void PlayEnd()
    {
        await UniTask.WaitUntil(() => _anim.IsStop(0));// GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
        //Invoke("FadeOut", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
