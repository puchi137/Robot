using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeHealth(float health)
    {
        this.health -= health;
        if(this.health <= 0) Death();
    }
    public abstract void Death();

}
