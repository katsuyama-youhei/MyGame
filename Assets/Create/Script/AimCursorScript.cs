using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AimCursorScript : MonoBehaviour
{
    public GameObject cursorUI;
    public float cursorDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float rightStickX = Input.GetAxis("RightStickHorizontal");
        float rightStickY = Input.GetAxis("RightStickVertical");

        Vector3 cursorPosition;

        if (rightStickX != 0 || rightStickY != 0)
        {
            // ���͂Ɋ�Â��ăG�C���������v�Z
            Vector2 stickDirection = new Vector2(rightStickX, rightStickY).normalized;

            // �J�[�\�����v���C���[�̑O���ɔz�u�iUI���W�Łj
            Vector3 centerPosition = new Vector3(transform.position.x, transform.position.y+ 0.8f, transform.position.z-1.0f);
            cursorPosition = centerPosition + new Vector3(stickDirection.x, stickDirection.y, 0) * cursorDistance;
            cursorUI.transform.position = cursorPosition;

            // �J�[�\�����X�e�B�b�N�̓��͕����ɉ�]������
            float angle = Mathf.Atan2(rightStickY, rightStickX) * Mathf.Rad2Deg;
            cursorUI.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // �v���C���[�̑O�������Ɋ�Â��ăJ�[�\���̈ʒu���v�Z
            Vector3 playerForward = transform.forward;

            // �J�����̌����ɍ��킹�ăv���C���[�̑O���������v�Z����
            Vector3 forwardDirection = new Vector3(playerForward.x, playerForward.y, 0).normalized;

            Vector3 centerPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z-1.0f);

            // �v���C���[�O���ɃJ�[�\����z�u
            cursorPosition = centerPosition + forwardDirection * cursorDistance;

            cursorUI.transform.position = cursorPosition;

            // �J�[�\�����v���C���[�̌����ɉ����ĉ�]������
            float angle = Mathf.Atan2(forwardDirection.z, forwardDirection.x) * Mathf.Rad2Deg;
            cursorUI.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
