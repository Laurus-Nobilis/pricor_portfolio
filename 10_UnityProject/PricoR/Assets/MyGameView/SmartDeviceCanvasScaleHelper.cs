using UnityEngine;
using UnityEngine.UI;
[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
public class SmartDeviceCanvasScaleHelper : MonoBehaviour
{
    // 例として iPhone 5 のピクセル数を基準とする
    [SerializeField] private float standardWidth = 640.0f;
    [SerializeField] private float standardHeight = 1136.0f;
    private CanvasScaler scaler; private Canvas canvas;
    private void Start() { UpdateScaler(); }
#if UNITY_EDITOR
    private void OnEnable() { UnityEditor.EditorApplication.update += UpdateScaler; }
    private void OnDisable() { UnityEditor.EditorApplication.update -= UpdateScaler; }
    private void Update() { UpdateScaler(); }
#endif

    private void UpdateScaler()
    {
        if (scaler == null)
        {
            scaler = GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(standardWidth, standardHeight);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
        if (canvas == null) { canvas = GetComponent<Canvas>(); }
        if (canvas.pixelRect.width == 0 || canvas.pixelRect.height == 0) { return; }
        float standardAspectRatio = standardWidth / standardHeight; float currentAspectRatio = canvas.pixelRect.width / canvas.pixelRect.height;
        // 縦が短いデバイスはタブレット端末とみなす
        if (currentAspectRatio > standardAspectRatio)
        {
            // 高さにマッチさせる（横に隙間が出来る）
            scaler.matchWidthOrHeight = 1;
        }
        else
        {
            // 幅にマッチさせる（縦に伸びる）
            scaler.matchWidthOrHeight = 0;
        }
    }
}