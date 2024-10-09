using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AimCursorScript : MonoBehaviour
{
    public RectTransform cursorUI;
    //public Transform player;
    public Camera mainCamera;
    public float cursorDistance = 2f;
    public float sensitivity = 5f;

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
            // 入力に基づいてエイム方向を計算
            Vector2 stickDirection = new Vector2(rightStickX, rightStickY).normalized;

            // カーソルをプレイヤーの前方に配置（UI座標で）
            Vector3 centerPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
            cursorPosition = centerPosition + new Vector3(stickDirection.x, stickDirection.y, 0) * cursorDistance;
            cursorUI.position = Camera.main.WorldToScreenPoint(cursorPosition);

            // カーソルをスティックの入力方向に回転させる
            float angle = Mathf.Atan2(rightStickY, rightStickX) * Mathf.Rad2Deg;
            cursorUI.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // プレイヤーの前方方向に基づいてカーソルの位置を計算
            Vector3 playerForward = transform.forward;

            // カメラの向きに合わせてプレイヤーの前方方向を計算する
            Vector3 forwardDirection = new Vector3(playerForward.x, playerForward.y, 0).normalized;

            Vector3 centerPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);

            // プレイヤー前方にカーソルを配置
            cursorPosition = centerPosition + forwardDirection * cursorDistance;

            // カーソルをプレイヤーの向きに応じて回転させる
            float angle = Mathf.Atan2(forwardDirection.z, forwardDirection.x) * Mathf.Rad2Deg;
            cursorUI.rotation = Quaternion.Euler(0, 0, angle);
        }

        // カーソル位置を画面上に反映
        cursorUI.position = mainCamera.WorldToScreenPoint(cursorPosition);

    }
}
