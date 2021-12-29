using UnityEngine;
using UnityEngine.UI;
[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
public class SmartDeviceCanvasScaleHelper : MonoBehaviour
{
    // ��Ƃ��� iPhone 5 �̃s�N�Z��������Ƃ���
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
        // �c���Z���f�o�C�X�̓^�u���b�g�[���Ƃ݂Ȃ�
        if (currentAspectRatio > standardAspectRatio)
        {
            // �����Ƀ}�b�`������i���Ɍ��Ԃ��o����j
            scaler.matchWidthOrHeight = 1;
        }
        else
        {
            // ���Ƀ}�b�`������i�c�ɐL�т�j
            scaler.matchWidthOrHeight = 0;
        }
    }
}