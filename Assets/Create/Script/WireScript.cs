using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WireScript : MonoBehaviour
{
    public float wireRange = 100f;
    public LayerMask grappleableLayers;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private LineRenderer lineRenderer;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
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
           // MoveTowardsGrapplePoint();
            DrawWire();
        }

        if (Input.GetAxis("Fire3") != 0)  // ワイヤーを解除する
        {
            ReleaseWire();
        }
    }

    void ShootWire()
    {
        RaycastHit hit;
        // var direction = transform.rotation * Vector3.forward;

        Vector3 rayDirection = transform.forward;
        Vector3 rayTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);

        Debug.DrawRay(rayTransformPosition, rayDirection * wireRange, Color.red, 2f);  // レイを赤色で描画

        Debug.Log("Raycast start position: " + rayTransformPosition);

      /*  var forward = transform.forward;

        var right = transform.right;

        // Vector3.forwardに姿勢のクォータニオンを掛ける
        var forward2 = transform.rotation * Vector3.forward;

        // Vector3.forwardをローカル座標系からワールド座標系に変換
        var forward3 = transform.TransformDirection(Vector3.forward);

        Debug.Log($"1:{forward}, 2:{forward2}, 3:{forward3}");
        Debug.Log("right" + right);*/

        if (Physics.Raycast(rayTransformPosition, transform.forward, out hit, wireRange, grappleableLayers))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;
            Debug.Log("AAA");
        }
        else
        {
            Debug.Log("Raycast missed");
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

    void ApplyGrappleForce()
    {
        Vector3 directionToGrapplePoint = (grapplePoint - transform.position).normalized;
        float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);

        // キャラクターをワイヤーの固定点に引き寄せる力を加える
        rb.AddForce(directionToGrapplePoint * distanceToGrapplePoint * 10f);

        if (distanceToGrapplePoint < 1f)
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
