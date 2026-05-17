using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_Detection : MonoBehaviour
{
    public ParticleSystem dirt;
    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        dirt.Play();
    }
}
