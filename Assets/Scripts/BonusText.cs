using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusText : MonoBehaviour
{
    private float disappearTimer;
    private TextMeshPro textmesh;
    private Color color;
    
    void Start()
    {
        textmesh = transform.GetComponent<TextMeshPro>();
        color = textmesh.color;
    }

    void Update()
    {
        float moveYSpeed = 20f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer<0)
        {
            float disappearSpeed = 3f;
            color.a -= disappearSpeed * Time.deltaTime;
            textmesh.color = color;
            if (color.a <0)
            {
                Destroy(gameObject);
            }

        }

    }
}
