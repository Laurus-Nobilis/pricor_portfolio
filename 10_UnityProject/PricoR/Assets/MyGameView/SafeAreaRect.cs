
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class SafeAreaRect : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Rect safeArea = new Rect(0, 0, 1, 1);

#if UNITY_EDITOR
    private void OnEnable()
    {
        UnityEditor.EditorApplication.update += UpdateSafeArea;
    }
    private void OnDisable()
    {
        UnityEditor.EditorApplication.update -= UpdateSafeArea;
    }

    // Editor ��ŉ𑜓x�����������ꍇ�� false ��Ԃ�
    private bool IsScreenResolutionCorrect()
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        if (canvas.pixelRect.width == 0 || canvas.pixelRect.height == 0)
        {
            return false;
        }
        string[] editorScreenRes = UnityEditor.UnityStats.screenRes.Split('x');
        if (editorScreenRes.Length >= 2)
        {
            if (int.TryParse(editorScreenRes[0], out int editorScreenResWidth))
            {
                if (int.TryParse(editorScreenRes[1], out int editorScreenResHeight))
                {
                    if (Screen.width == editorScreenResWidth && Screen.height == editorScreenResHeight)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
#endif
    private void Update()
    {
        // �E�ꕔ�� Android �[���ł� Start() ���ł� Screen.safeArea ���������l��Ԃ��Ă��Ȃ��\��������̂ŁAUpdate() ���� Screen.safeArea ���ς���Ă��Ȃ������`�F�b�N���Ă���B
        UpdateSafeArea();
    }
    private void UpdateSafeArea()
    {
#if UNITY_EDITOR
        if (!IsScreenResolutionCorrect())
        { return; }
#else
        if (safeArea == Screen.safeArea) { return; }
#endif 
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        if (rectTransform == null)
        {
            rectTransform = transform as RectTransform;
        }
        float anchorMinX = Screen.safeArea.position.x / canvas.pixelRect.width;
        float anchorMinY = Screen.safeArea.position.y / canvas.pixelRect.height;
        float anchorMaxX = (Screen.safeArea.position.x + Screen.safeArea.size.x) / canvas.pixelRect.width;
        float anchorMaxY = (Screen.safeArea.position.y + Screen.safeArea.size.y) / canvas.pixelRect.height;
        rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY); rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        safeArea = Screen.safeArea;
    }
}
