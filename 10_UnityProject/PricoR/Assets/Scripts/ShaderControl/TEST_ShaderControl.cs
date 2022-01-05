using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Assertions;

public class TEST_ShaderControl : MonoBehaviour
{
    float _threshold = 0.5f;
    int _thresholdId;

    Material _material; //共有されてるマテリアル情報
    Material _myMaterial;   //アタッチしているオブジェクトに固有のマテリアル

    // Start is called before the first frame update
    void Start()
    {
        // Material を操作すると、それを利用しているMaterial全てに影響してしまう。
        var img = GetComponent<Image>();
        _material = img.material;

        //_myMaterial = img.canvasRenderer.renderer
        //Assert.IsNotNull(_myMaterial);

        _threshold = 0;
        _thresholdId = Shader.PropertyToID("_Threshold");//???:複数のシェーダーに同名の物が重複した場合はどうなる？

         //Tween();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void Tween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(
            DOTween.To(() => _threshold, x => Fill(x), 1f, 1f).SetLoops(2)
            .SetDelay(0.5f));
        seq.Append(
            DOTween.To(x => Fill(x), 0f, 1f, 2f).SetLoops(2)
            .SetEase(Ease.OutQuart)
            );
        //DOTween.To(() => _threshold, x => Fill(x), 1f, 2f).Loops();
        seq.SetLoops(-1).Play();
    }

    void Fill(float v, Material mat)
    {
        mat.SetFloat(_thresholdId, v);
    }
    void Fill(float v)
    {
        _material.SetFloat(_thresholdId, v);
    }
    async void Move()
    {
        await Loop();
    }
    async UniTask Loop()
    {
        //await transform.DOMoveX(0f, 1f);
        //await transform.DOMoveX(-1.0f, 2.0f);

        return;
    }
}
