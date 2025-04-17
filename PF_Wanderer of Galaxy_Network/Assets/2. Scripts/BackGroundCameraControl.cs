using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCameraControl : MonoBehaviour
{
    PlayerControl _player;
    Rigidbody _playerRgBody;
    // Start is called before the first frame update
    void Start()
    {
        _playerRgBody = GameObject.FindGameObjectWithTag("PlayerRoot").GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += _playerRgBody.velocity * 2.5f * Time.deltaTime;
        if(transform.localPosition.x < -1000)
        {
            Vector3 pos = transform.localPosition;
            pos.x = 900;
            transform.localPosition = pos;
        }
        else if(transform.localPosition.x > 1000)
        {
            Vector3 pos = transform.localPosition;
            pos.x = -900;
            transform.localPosition = pos;
        }
        if(transform.localPosition.z < -1200)
        {
            Vector3 pos = transform.localPosition;
            pos.z = 1100;
            transform.localPosition = pos;
        }
        else if(transform.localPosition.z > 1200)
        {
            Vector3 pos = transform.localPosition;
            pos.z = -1100;
            transform.localPosition = pos;
        }
    }
}
