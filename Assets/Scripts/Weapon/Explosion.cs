using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem[] explosion;
    private AudioSource explosionAudio;
    private void Start()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Play();
        }  
        StartCoroutine(Wait(3));
    }
    IEnumerator Wait(int delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
