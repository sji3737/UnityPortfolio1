using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_ActivationSequence : MonoBehaviour {

    public GameObject activate;
    public float atThisTime;
    private float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > atThisTime) activate.SetActive(true);
        Destroy(this);



	}
}
