using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorWall : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Renderer quadRend;
    private Color pickupColor;
    private Color wallColor;
    private AudioSource au;
    private int colorRandom;

    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
        colorRandom = Random.Range(0, 4);
        switch (colorRandom)
        {
            case 0:
                pickupColor = new Color(1, 0, 0, 1);
                wallColor = new Color(1, 0, 0, 0.6f);
                break;
            case 1:
                pickupColor = new Color(0, 1, 0, 1);
                wallColor = new Color(0, 1, 0, 0.6f);
                break;
            case 2:
                pickupColor = new Color(1, 0, 1, 1);
                wallColor = new Color(1, 0, 1, 0.6f);
                break;
            case 3:
                pickupColor = new Color(0, 1, 1, 1);
                wallColor = new Color(0, 1, 1, 0.6f);
                break;
        }

        foreach (var c in transform.GetComponentsInChildren<Renderer>())
        {
            c.material.color = pickupColor;
        }

        quadRend.material.color = wallColor;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerController.SetColor(colorRandom);
            au.Play();
        }
    }
}