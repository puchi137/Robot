using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        ParticleSystem hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
        hit.Play();
        if(collision.collider.GetComponent<Enemy>() != null)
        {
            collision.collider.GetComponent<Enemy>().TakeHealth(damage);
        }
        Destroy(gameObject);
    }
    public void bulletDamege(float damage)
    { this.damage = damage; }   
}
