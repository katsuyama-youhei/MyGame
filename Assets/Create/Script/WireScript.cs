using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WireScript : MonoBehaviour
{
    public float wireRange = 50f;
    public LayerMask grappleableLayers;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            ShootWire();
        }

        if (isGrappling)
        {
            MoveTowardsGrapplePoint();
            DrawWire();
        }

        if (Input.GetAxis("Fire2") != 0)  // ワイヤーを解除する
        {
            ReleaseWire();
        }
    }

    void ShootWire()
    {
        RaycastHit hit;
        // var direction = transform.rotation * Vector3.forward;
        
        if (Physics.Raycast(transform.position, transform.right, out hit, wireRange, grappleableLayers))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;
            Debug.Log("AAA");
        }
    }

    void MoveTowardsGrapplePoint()
    {
        // 自機をワイヤーのポイントに引き寄せる
        Vector3 moveDirection = grapplePoint - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, grapplePoint, Time.deltaTime * 10f);

        // 接近しすぎたらワイヤーを解除
        if (Vector3.Distance(transform.position, grapplePoint) < 1f)
        {
            ReleaseWire();
        }
    }
    void DrawWire()
    {
        lineRenderer.SetPosition(0, transform.position);  // 自機の位置
        lineRenderer.SetPosition(1, grapplePoint);        // ワイヤーの固定位置
    }

    void ReleaseWire()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
    }
}
