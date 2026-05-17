using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    public AudioSource step;
    public void PlayStep()
    {
        step.Play();
    }
}
