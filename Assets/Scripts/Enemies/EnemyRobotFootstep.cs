using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotFootstep : MonoBehaviour
{
    public AudioSource footstepAudio;
    public void PlayFootstep()
    {
        footstepAudio.Play();
    }
}
