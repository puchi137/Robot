using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody rb;
    public ParticleSystem smoke;
    public float force;
    public GameObject explosion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        smoke.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.forward = rb.velocity;
    }

}
