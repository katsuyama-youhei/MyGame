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
    public float pullSpeed = 10f;
    public float releaseJump = 2f;

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

        if (Input.GetAxis("Fire3") != 0)  // ���C���[����������
        {
            ReleaseWire();
        }
    }

    void ShootWire()
    {
        RaycastHit hit;

        Vector3 rayDirection = transform.forward;
        Vector3 rayTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);

        Debug.DrawRay(rayTransformPosition, rayDirection * wireRange, Color.red, 2f);  // ���C��ԐF�ŕ`��

        Debug.Log("Raycast start position: " + rayTransformPosition);

        if (Physics.Raycast(rayTransformPosition, transform.forward, out hit, wireRange, grappleableLayers))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            rb.useGravity = false;  // �d�͂𖳌��ɂ���
            lineRenderer.enabled = true;
        }
        else
        {
            Debug.Log("Raycast missed");
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

    void ApplyGrappleForce()
    {
        Vector3 directionToGrapplePoint = (grapplePoint - transform.position).normalized;
        // float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);

        // �L�����N�^�[�����C���[�̌Œ�_�Ɉ����񂹂�͂�������
        // rb.AddForce(directionToGrapplePoint * distanceToGrapplePoint * 10f);

        /*if (distanceToGrapplePoint < 1f)
        {
            ReleaseWire();
        }*/

        rb.velocity = directionToGrapplePoint * pullSpeed;

        float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);
        if (distanceToGrapplePoint < 1f)
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
        rb.useGravity = true;  // �d�͂��ĂїL���ɂ���
        lineRenderer.enabled = false;
        rb.AddForce(Vector3.up * releaseJump, ForceMode.Impulse);
    }
}
