using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WireScript : MonoBehaviour
{
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private LineRenderer lineRenderer;
    private Rigidbody playerrb;

    public Transform cameraTransform;
    public float wireRange = 100f;
    public float pullSpeed = 10f;
    public float releaseJump = 2f;
    public LayerMask grappleableLayers;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerrb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            ApplyGrappleForce();
        }
    }

    void Shoot()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            ShootWire();
        }

        if (isGrappling)
        {
            DrawWire();
        }

        if (Input.GetAxis("Fire3") != 0)  // ワイヤーを解除する
        {
            StopGrapple();
        }
    }

    void ShootWire()
    {
        float rightStickX = Input.GetAxis("RightStickHorizontal");
        float rightStickY = Input.GetAxis("RightStickVertical");

        Vector3 rightStickDirection = new Vector3(rightStickX, rightStickY, 0).normalized;
        Vector3 grappleDirection = cameraTransform.TransformDirection(rightStickDirection);

        RaycastHit hit;

        Vector3 rayTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);

        //Debug.DrawRay(rayTransformPosition, grappleDirection * wireRange, Color.red, 2f);  // レイを赤色で描画

        Debug.Log("Raycast start position: " + rayTransformPosition);

        if(rightStickX !=0||rightStickY !=0)
        {
            if (Physics.Raycast(rayTransformPosition, grappleDirection, out hit, wireRange, grappleableLayers))
            {
                grapplePoint = hit.point;
                isGrappling = true;
                playerrb.useGravity = false;
                lineRenderer.enabled = true;
            }
            else
            {
                Debug.Log("Raycast missed");
            }
        }
        else
        {
            if (Physics.Raycast(rayTransformPosition, transform.forward, out hit, wireRange, grappleableLayers))
            {
                grapplePoint = hit.point;
                isGrappling = true;
                playerrb.useGravity = false;  // 重力を無効にする
                lineRenderer.enabled = true;
            }
            else
            {
                Debug.Log("Raycast missed");
            }
        }
       
        
    }

    void ApplyGrappleForce()
    {
        Vector3 directionToGrapplePoint = (grapplePoint - transform.position).normalized;
        // float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);

        // キャラクターをワイヤーの固定点に引き寄せる力を加える
        // rb.AddForce(directionToGrapplePoint * distanceToGrapplePoint * 10f);

        /*if (distanceToGrapplePoint < 1f)
        {
            ReleaseWire();
        }*/

        playerrb.velocity = directionToGrapplePoint * pullSpeed;

        float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);
        if (distanceToGrapplePoint < 1f)
        {
            ReleaseWire();
        }
    }

    void DrawWire()
    {
        Vector3 rayTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        lineRenderer.SetPosition(0, rayTransformPosition);  // 自機の位置
        lineRenderer.SetPosition(1, grapplePoint);        // ワイヤーの固定位置
    }

    void ReleaseWire()
    {
        isGrappling = false;
        playerrb.useGravity = true;
        lineRenderer.enabled = false;
        playerrb.AddForce(Vector3.up * releaseJump, ForceMode.Impulse);
    }

    void StopGrapple()
    {
        isGrappling = false;
        playerrb.useGravity = true;
        lineRenderer.enabled = false;
    }
}
