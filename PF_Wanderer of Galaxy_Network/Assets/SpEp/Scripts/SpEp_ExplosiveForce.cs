using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_ExplosiveForce : MonoBehaviour {

    
    public float pushForce=5000.0f;
    public float duration = 0.5f;
    private float time = 0.0f;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > duration) Destroy(this);
    }

    void OnTriggerStay(Collider other)
        {
        
        if (other.attachedRigidbody)
            
            other.attachedRigidbody.AddForce((other.transform.position - transform.position).normalized * pushForce * Time.deltaTime);
            
        }

    
}
