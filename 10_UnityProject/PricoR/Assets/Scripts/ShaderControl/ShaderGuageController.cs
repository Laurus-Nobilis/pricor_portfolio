using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Renderer))]
public class ShaderGuageController : MonoBehaviour
{
    float _threshold = 0.5f;
    int _thresholdId;

    Material _material;

    // Start is called before the first frame update
    void Start()
    {
        // Renderer のマテリアルを操作すると、個別に変更できる。（MaterialのシェーダーのProperty変更でもドローコールが増えそう？？）
        var img = GetComponent<Renderer>();
        _material = img.material;
        _threshold = 0;
        _thresholdId = Shader.PropertyToID("_Threshold");

         Tween();
    }

    async void Tween()
    {
        var task = TweenTask();
    }
    async UniTask TweenTask()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(
            DOTween.To(() => _threshold, x => Fill(x), 1f, 1f).SetLoops(2)
            .SetDelay(0.2f));
        await seq.SetLoops(-1).Play();
    }

    void Fill(float v)
    {
        _material.SetFloat(_thresholdId, v);
    }
}
