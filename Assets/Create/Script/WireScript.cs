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
    private Vector3 rayTransformPosition;
    private Vector2 rightStick;
    private Vector2 distance = new Vector2(0.4f, 1f);
    private bool isVertical = false;
    private bool isForward = true;

    public Transform cameraTransform;
    public float wireRange = 100f;
    public float pullSpeed = 10f;
    public float releaseJump = 2f;
    public LayerMask grappleableLayers;

    public GameObject cursor;

    //private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerrb = GetComponent<Rigidbody>();
       //playerScript = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        rightStick.x = Input.GetAxis("RightStickHorizontal");
        rightStick.y = Input.GetAxis("RightStickVertical");
        /*if (!playerScript.isCollisionBlock)
        {
            Debug.Log("CollisionBlock");
            Shoot();
        }*/
        if (isGrappling)
        {
            DrawWire();
        }

        if (Input.GetAxis("Fire3") != 0)  // ワイヤーを解除する
        {
            StopGrapple();
        }
    }

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            ApplyGrappleForce();
        }
    }

    public void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isGrappling)
            {
                ShootWire();
            }
        }
    }

    void ShootWire()
    {
        Vector3 rightStickDirection = new Vector3(rightStick.x, rightStick.y, 0).normalized;
        Vector3 grappleDirection = cameraTransform.TransformDirection(rightStickDirection);

        RaycastHit hit;
        Vector3 direction = transform.forward;

        rayTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);

        if (rightStick.x == 0)
        {
            if (rightStick.y == 0)
            {
                isVertical = false;
                if (transform.forward.x > 0)
                {
                    isForward = true;
                }
                else if (transform.forward.x < 0)
                {
                    isForward = false;
                }
                else
                {
                    if (direction.x > 0)
                    {
                        isForward = true;
                    }
                    else
                    {
                        isForward = false;
                    }
                }
            }
            else if (rightStick.y > 0)
            {
                isVertical = true;
                isForward = true;
            }
            else
            {
                isVertical = true;
                isForward = false;
            }
        }
        else if (rightStick.x > 0)
        {
            isVertical = false;
            isForward = true;
        }
        else if (rightStick.x < 0)
        {
            isVertical = false;
            isForward = false;
        }

        Debug.Log("Raycast start position: " + rayTransformPosition);

        if (rightStick.x != 0 || rightStick.y != 0)
        {
            if (Physics.Raycast(rayTransformPosition, grappleDirection, out hit, wireRange, grappleableLayers))
            {
                grapplePoint = hit.point;
                isGrappling = true;
                playerrb.useGravity = false;
                lineRenderer.enabled = true;
                cursor.SetActive(false);
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
                playerrb.useGravity = false;
                lineRenderer.enabled = true;
                cursor.SetActive(false);
            }
            else
            {
                Debug.Log("Raycast missed");
            }
        }

        Debug.Log("Raycast grappleposition: " + grapplePoint);
    }

    void ApplyGrappleForce()
    {
        Vector3 directionToGrapplePoint = (grapplePoint - transform.position).normalized;

        playerrb.velocity = directionToGrapplePoint * pullSpeed;

        float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);
        if (distanceToGrapplePoint < 2f)
        {
            ReleaseWire();
        }
    }

    void DrawWire()
    {
        Vector3 rayTransformPosition;

        if (isVertical) // 真上or真下
        {
            if (isForward) // 真上
            {
                rayTransformPosition = new Vector3(transform.position.x, transform.position.y + distance.y + 0.7f, transform.position.z);
                lineRenderer.SetPosition(0, rayTransformPosition);  // 自機の位置
                lineRenderer.SetPosition(1, grapplePoint);        // ワイヤーの固定位置
            }
            else // 真下
            {
                rayTransformPosition = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
                lineRenderer.SetPosition(0, rayTransformPosition);  // 自機の位置
                lineRenderer.SetPosition(1, grapplePoint);        // ワイヤーの固定位置
            }
        }
        else
        {
            if (isForward)
            {
                rayTransformPosition = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
                lineRenderer.SetPosition(0, rayTransformPosition);  // 自機の位置
                lineRenderer.SetPosition(1, grapplePoint);        // ワイヤーの固定位置
            }
            else
            {
                rayTransformPosition = new Vector3(transform.position.x - distance.x, transform.position.y + distance.y, transform.position.z);
                lineRenderer.SetPosition(0, rayTransformPosition);  // 自機の位置
                lineRenderer.SetPosition(1, grapplePoint);        // ワイヤーの固定位置
            }
        }

    }

    void ReleaseWire()
    {
        isGrappling = false;
        playerrb.useGravity = true;
        lineRenderer.enabled = false;
        playerrb.AddForce(Vector3.up * releaseJump, ForceMode.Impulse);
        cursor.SetActive(true);
    }

    void StopGrapple()
    {
        isGrappling = false;
        playerrb.useGravity = true;
        lineRenderer.enabled = false;
        cursor.SetActive(true);
    }
}
