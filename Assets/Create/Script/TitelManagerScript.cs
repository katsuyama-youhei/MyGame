using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitelManagerScript : MonoBehaviour
{
    public static TitelManagerScript Instance { get; private set; }
    public bool isStart = false;
    public GameObject capsule;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("複数のTargetScriptインスタンスが存在します！");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") != 0)
        {
            isStart = true;
            capsule.SetActive(true);

        }
    }
}
