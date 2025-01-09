using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private float alphaValue=0.2f;
    private bool isChange = true;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
         spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        if (isChange)
        {
            alphaValue += 0.002f;
            color.a = alphaValue;
            spriteRenderer.color = color;
            if (alphaValue >= 0.9)
            {
                isChange = false;
            }
        }
        else
        {
            alphaValue -= 0.001f;
            color.a = alphaValue;
            spriteRenderer.color = color;
            if (alphaValue <= 0.2)
            {
                isChange = true;
            }
        }
        Debug.Log(alphaValue);
    }
}
