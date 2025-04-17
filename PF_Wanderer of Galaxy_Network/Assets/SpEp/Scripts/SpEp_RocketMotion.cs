using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_RocketMotion : MonoBehaviour {

    public AnimationCurve sideMovement;
    private float time;
    private float curValue;
    private Rigidbody rb;
    public float strMulti;
    public bool randomStart = false;
    public float timeMulti=1;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        if (randomStart) time = Random.Range(0.0f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime*timeMulti;
        curValue = sideMovement.Evaluate(time);
        rb.AddRelativeForce(Vector3.right * curValue * Time.deltaTime*strMulti);
    }
}
