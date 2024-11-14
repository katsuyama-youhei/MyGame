using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationScript : MonoBehaviour
{

    private float bounceForce = 7f;
    private float centerPullForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // プレイヤーのY軸の位置とCubeのY軸の中心を比較
                Vector3 cubeCenter = transform.position;
                Vector3 playerPosition = other.transform.position;

                if (playerPosition.y > cubeCenter.y)
                {
                    // 上から接触 -> Y軸に跳ね返し力を加える
                    playerRb.velocity = new Vector3(playerRb.velocity.x, bounceForce, playerRb.velocity.z);
                }
                else
                {
                    // 下から接触 -> Cubeの中心に向かって加速
                    Vector3 directionToCenter = (cubeCenter - playerPosition).normalized;
                    playerRb.velocity = directionToCenter * centerPullForce;
                }
            }
        }
    }

}
