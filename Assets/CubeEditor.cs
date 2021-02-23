using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeEditor : MonoBehaviour
{
    [SerializeField] private float gridSize = 10f;
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject quad;
    private TextMeshPro textMesh;

    private void Update()
    {
        Vector3 snapPos;
        textMesh = GetComponentInChildren<TextMeshPro>();

        snapPos.z = Mathf.RoundToInt(transform.position.z / gridSize) * gridSize;
        transform.position = new Vector3(0f, 0f, snapPos.z);

        string labelText = "x" + (snapPos.z / 100 - 5);
        textMesh.text = labelText;
        gameObject.name = labelText;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            quad.SetActive(false);
            pc.SetScoreMultiply(transform.position.z / 100 - 5, transform);
        }
    }
}