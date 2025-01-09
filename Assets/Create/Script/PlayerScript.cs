using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    public float jumpForce = 1.0f;
    public float moveSpeed = 1.0f; // ˆÚ“®‘¬“x
    public float forceMultiplier = 1f;

    private Rigidbody rb;
    private float distance = 0.72f;
    public bool isCollisionBlock = true;

    public Animator animator;

    private WireScript wireScript;

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wireScript = GetComponent<WireScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // ƒŒƒC‚ð‚µ‚½•ûŒü‚É”ò‚Î‚·
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.4f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);
        isCollisionBlock = Physics.Raycast(ray, distance);

        if (isCollisionBlock)
        {
            Jump();
        }


        wireScript.Shoot();

        if (transform.position.y <= -5)
        {
            canvas.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");

        if (move < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            animator.SetBool("isWalk", true);
        }
        else if (move > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }

        if (Mathf.Abs(rb.velocity.x) < moveSpeed)
        {
            Vector3 force = new Vector3(move * forceMultiplier, 0, 0);
            rb.AddForce(force, ForceMode.Force);
        }
        Vector3 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
        rb.velocity = velocity;
    }

    void Jump()
    {
        if (Input.GetAxis("Jump") != 0)
        {
            Vector3 v = rb.velocity;

            v.y = jumpForce;

            rb.velocity = v;
        }
    }
}
