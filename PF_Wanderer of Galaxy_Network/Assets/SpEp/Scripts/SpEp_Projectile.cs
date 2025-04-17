using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_Projectile : MonoBehaviour {
    public float startSpeed = 1;
    public float continuousSpeed = 0;
    public ParticleSystem particleSystemRoot;
    public GameObject[] instantlyDestroy;
    public GameObject muzzleFire;
    public int damage = 1;

    private Rigidbody rb;

    public bool detachToWorld = true;

    public GameObject explosionEffect;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * startSpeed, ForceMode.Impulse);
        if (muzzleFire) Instantiate(muzzleFire, transform.position, transform.rotation);
    }
	
	// Update is called once per frame
	void Update () {
        if (detachToWorld) transform.parent = null;

        rb.AddRelativeForce(Vector3.forward * continuousSpeed * Time.deltaTime);
    }
    void OnCollisionEnter(Collision other)
    {

        continuousSpeed = 0;
        startSpeed = 0;
        rb.isKinematic = true;
        rb.detectCollisions = false;
        particleSystemRoot.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        for (int i = 0; i < instantlyDestroy.Length; i++)
        {
            Destroy(instantlyDestroy[i]);
        }

        if (explosionEffect) Instantiate(explosionEffect, transform.position, transform.rotation);

        if (other.gameObject.GetComponent<SpEp_Enemy>())
        {
            other.gameObject.GetComponent<SpEp_Enemy>().HP -= damage;
            other.gameObject.GetComponent<SpEp_Enemy>().TakingDamage();
        }

        Destroy(gameObject, 3);
    }
    }
