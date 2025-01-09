using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMoveScript : MonoBehaviour
{

    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TitelManagerScript.Instance.isStart)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
