using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerRoot");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _followPos = _player.transform.position;
        _followPos.y = Camera.main.transform.position.y;
        Camera.main.transform.position = _followPos;
    }
}
