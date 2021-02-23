using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private PlayerController _pc;
    [SerializeField] private chibiKick ck;
    private float smoothSpeed = 0.125f;
    private Vector3 smoothedPos, desiredPos;
    [SerializeField] private Vector3 offset, powerUpOffset, bonusOffset, kickOffset,collectCountOffset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ck.getKick())
        {
            target = _pc.GetLastCollect();
            if (target == null) return;
            smoothSpeed = 0.05f;
            desiredPos = target.position + bonusOffset;
            smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_pc.GetFinish())
        {
            desiredPos = target.position + kickOffset;
            smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
            transform.rotation = Quaternion.Euler(0, -50, 0);
        }
        else if (target.tag == "Player")
        {
            desiredPos = target.position + offset;
            smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
            if (_pc.GetCollectsCount() >50 && !_pc.GetPowerUp())
            {
                desiredPos = target.position + collectCountOffset;
                smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
                transform.position = smoothedPos;
                transform.rotation = Quaternion.Euler(30,-15,0);
            }
            if (_pc.GetPowerUp())
            {
                Vector3 powerUpPos = target.position + powerUpOffset;
                transform.position = powerUpPos;
                transform.rotation = Quaternion.Euler(0,-16.7f,0);
            }
        }
    }
}