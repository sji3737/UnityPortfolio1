using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_CamTarget : MonoBehaviour {
    public GameObject myTarget;
    public float speed = 20f;
    public float step = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        step = speed * Time.deltaTime * Vector3.Distance(myTarget.transform.position, transform.position);
        transform.position=(Vector3.MoveTowards(transform.position, myTarget.transform.position, step));
    }
}
