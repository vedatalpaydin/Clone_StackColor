using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float rightLeftSpeed = 10f;
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody rb;
    private Vector2 lastMousePos, delta;
    private bool gameIsStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            gameIsStart = true;
            
        }

        if (gameIsStart)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePos = Input.mousePosition;
                delta = currentMousePos - lastMousePos;
                lastMousePos = currentMousePos;
                transform.position =
                    new Vector3(Mathf.Clamp(transform.position.x + delta.x * Time.deltaTime * rightLeftSpeed, -4, 4),
                        transform.position.y, transform.position.z);                
            }
        }
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    void MoveForward()
    {
        if (gameIsStart && rb.velocity.z<20f)
            rb.AddForce(transform.forward*moveSpeed);
    }
}