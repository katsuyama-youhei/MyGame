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

        if (Input.GetAxis("Fire2") != 0)  // ���C���[����������
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
        // ���@�����C���[�̃|�C���g�Ɉ����񂹂�
        Vector3 moveDirection = grapplePoint - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, grapplePoint, Time.deltaTime * 10f);

        // �ڋ߂��������烏�C���[������
        if (Vector3.Distance(transform.position, grapplePoint) < 1f)
        {
            ReleaseWire();
        }
    }
    void DrawWire()
    {
        lineRenderer.SetPosition(0, transform.position);  // ���@�̈ʒu
        lineRenderer.SetPosition(1, grapplePoint);        // ���C���[�̌Œ�ʒu
    }

    void ReleaseWire()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
    }
}
