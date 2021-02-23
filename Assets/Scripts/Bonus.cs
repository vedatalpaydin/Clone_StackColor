using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject bonusText;
    private List<GameObject> otherGo = new List<GameObject>();
    private AudioSource au;

    private void Start()
    {
        au = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pickup" && !otherGo.Contains(other.gameObject))
        {
            if (pc.GetFinish())
            {
                Instantiate(bonusText, transform.position, Quaternion.identity);
                au.Play();
                pc.SetScoreText(5);
                pc.SetScoreMultiply(transform.position.z / 100 - 5, transform);
                otherGo.Add(other.gameObject);
            }
         
        }
    }
}