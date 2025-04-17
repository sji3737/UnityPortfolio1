using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_Enemy : MonoBehaviour {

    public int HP = 1;
    public GameObject explosion;
    public Animator damageAnim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (HP <= 0)
            {
            Destroyed();
        }
		
	}

    void Destroyed ()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void TakingDamage ()
    {
        if (damageAnim) damageAnim.Play("damageAnim", -1, 0);
    }
}
