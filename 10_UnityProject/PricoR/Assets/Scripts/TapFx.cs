using UnityEngine;

public class TapFx : MonoBehaviour
{
    [SerializeField] ParticleSystem tapEffect;              // �^�b�v�G�t�F�N�g
    [SerializeField] Camera _camera;                        // �J�����̍��W

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �}�E�X�̃��[���h���W�܂Ńp�[�e�B�N�����ړ����A�p�[�e�B�N���G�t�F�N�g��1��������
            var pos = _camera.ScreenToWorldPoint(Input.mousePosition + _camera.transform.forward);
            tapEffect.transform.position = pos;
            tapEffect.Emit(1);
        }
    }
}