using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpEp_SpaceshipControl : MonoBehaviour {

    public float speed = 200f;
    public float turn = 200f;
    public float tilt = 100f;

    public GameObject shootPoint;

    public Animator tilter;
    private float currTilt = 0.5f;
    public float tiltspeed = 1f;
    public float tiltDampen = 0.1f;

    private GameObject justCreated;
    private Rigidbody rb;

    public GameObject[] effects;
    private int currEffect=0;
    public Text text;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        tilter.Play("TiltAnimation", -1, currTilt);
        text.text = effects[currEffect].name;
    }
	
	// Update is called once per frame
	void Update () {

        tilter.Play("TiltAnimation", -1, currTilt);

        if (currTilt > 1f) currTilt = 1f;
        if (currTilt < 0f) currTilt = 0f;

        if (currTilt > 0.5f)
        {
            currTilt -= (tiltDampen);
        }

        if (currTilt < 0.5f)
        {
            currTilt += (tiltDampen);
                }

        if (currTilt > 0.49f && currTilt < 0.51f && (!Input.anyKey)) currTilt = 0.5f;

        if (Input.GetKey("w"))
        {
            rb.AddRelativeForce(Vector3.forward * speed*Time.deltaTime);
        }

        if (Input.GetKey("a"))
        {
            rb.AddRelativeTorque(Vector3.up * -turn * Time.deltaTime);
            currTilt -= tiltspeed * Time.deltaTime;

        }

        if (Input.GetKey("d"))
        {
            rb.AddRelativeTorque(Vector3.up * turn * Time.deltaTime);

            currTilt += tiltspeed * Time.deltaTime;
        }



        if (Input.GetKey("s"))
        {
            rb.AddRelativeForce(Vector3.forward * -speed * Time.deltaTime);
        }

        if (Input.GetKeyDown("space"))
        {
        
            justCreated=Instantiate(effects[currEffect], shootPoint.transform.position, shootPoint.transform.rotation);
            justCreated.transform.parent = shootPoint.transform;
            
        }

        if (Input.GetKeyDown("q"))
        {
            currEffect -= 1;
            if (currEffect < 0) currEffect = effects.Length - 1;
            text.text = effects[currEffect].name;
        }
        if (Input.GetKeyDown("e"))
        {
            currEffect += 1;
            if (currEffect > (effects.Length-1)) currEffect = 0;
            text.text = effects[currEffect].name;
        }


    }
}
