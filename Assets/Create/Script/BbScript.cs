using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class BbScript : MonoBehaviour
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

        if (transform.position.x >= -5f)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
